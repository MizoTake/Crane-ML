using System.Threading;
using CraneML.Extensions;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace CranML
{
    public class CraneGamePresenter : MonoBehaviour
    {

        [SerializeField] private CraneGameView view;

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
                .Subscribe(async _ =>
                {
                    await view.ArmOpen();
                    await view.ReturnPosition();
                });
        }
        
        public void HorizontalButtonPressed()
        {
            view.HorizontalButtonUp = false;
            view.MoveTo(Vector3.back * 0.005f, horizontalCancellation.Token).Forget();
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
            view.MoveTo(Vector3.left * 0.005f, verticalCancellation.Token).Forget();
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
