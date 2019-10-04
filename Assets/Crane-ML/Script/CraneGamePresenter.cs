using System;
using System.Threading;
using DG.Tweening;
using Doozy.Engine.UI;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace CranML
{
    public class CraneGamePresenter : MonoBehaviour
    {

        [SerializeField] private CraneGameView view;
        [SerializeField] private Animator armAnim;

        private CancellationTokenSource horizontalCancellation = new CancellationTokenSource();
        private CancellationTokenSource verticalCancellation = new CancellationTokenSource();

        private bool firstButonPressed = false;

        private void Start()
        {
            view.HorizontalButton.OnClickAsObservable()
                .Where(_ => !firstButonPressed)
                .TakeUntilDestroy(this)
                .Subscribe(_ => firstButonPressed = true);

            view.VerticalButton.OnClickAsObservable()
                .Where(_ => firstButonPressed)
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    view.ArmOpen();
                });
        }
        
        public void HorizontalButtonPressed()
        {
            view.HorizontalButtonUp = false;
            view.Move(Vector3.back * 0.005f, horizontalCancellation.Token).Forget();
        }
        
        public void HorizontalButtonUp()
        {
            horizontalCancellation.Cancel();
            horizontalCancellation = new CancellationTokenSource();
            view.HorizontalButtonUp = true;
        }

        public void VerticalButtonPressed()
        {
            view.VerticalButtonUp = false;
            view.Move(Vector3.left * 0.005f, verticalCancellation.Token).Forget();
        }
        
        public void VerticalButtonUp()
        {
            verticalCancellation.Cancel();
            verticalCancellation = new CancellationTokenSource();
            view.VerticalButtonUp = true;
        }

        private void OnDestroy()
        {
            horizontalCancellation.Cancel();
            verticalCancellation.Cancel();
        }
    }
}
