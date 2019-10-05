using System;
using System.Threading;
using CraneML;
using CraneML.Extensions;
using DG.Tweening;
using Doozy.Engine.UI;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace CranML
{
    public class CraneGameView : MonoBehaviour
    {
        
        [SerializeField] private CranActor arm;
        [SerializeField] private Animator armAnimation;
        [SerializeField] private UIButton horizontalUIButton;
        [SerializeField] private UIButton verticalUIButton;

        private Vector3 armInitPosition;
        
        public Button HorizontalButton => horizontalUIButton.Button;
        public Button VerticalButton => verticalUIButton.Button;

        public bool HorizontalButtonUp { get; set; }
        public bool VerticalButtonUp { get; set; }

        // TODO: 初期値からの移動場所
        // TODO: 移動制限(Clampとかで)

        private void Start()
        {
            armInitPosition = arm.transform.position;
        }

        public async UniTask MoveTo(Vector3 inputVector, CancellationToken token)
        {
            while(!HorizontalButtonUp || !VerticalButtonUp)
            {
                arm.transform.Translate(inputVector);
                await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime), cancellationToken: token);
            }
        }

        public async UniTask ArmOpen()
        {
//            armAnimation.speed = 0.2f;
            await DOTween.Sequence()
                .AppendCallback(() => armAnimation.enabled = false)
                .Append(arm.Open())
                .Append(arm.Down())
                .AppendInterval(0.5f)
                .Append(arm.Catch())
                .AppendInterval(1f)
                .Append(arm.transform.DOMoveY(1f, 3f).SetRelative().SetEase(Ease.OutQuad))
                .Play();
        }

        public async UniTask ReturnPosition()
        {
            await DOTween.Sequence()
                .AppendInterval(1f)
                .Append(arm.transform.DOMoveZ(armInitPosition.z, 3f))
                .Append(arm.transform.DOMoveX(armInitPosition.x, 3f))
                .AppendCallback(() => armAnimation.CrossFade("Open", 0.5f))
                .Play();
        }
    }
}
