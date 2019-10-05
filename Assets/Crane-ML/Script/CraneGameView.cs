using System;
using System.Threading;
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
        
        [SerializeField] private Transform arm;
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
            armInitPosition = arm.position;
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
            armAnimation.speed = 0.2f;
            await DOTween.Sequence()
                .Append(arm.transform.DOMoveY(-1f, 3f).SetRelative().SetEase(Ease.InQuad))
                .AppendInterval(0.5f)
                // animation speedを変える
//                .AppendCallback(() => armAnimation.CrossFade("Use", 0.5f))
                .AppendInterval(1f)
                .Append(arm.transform.DOMoveY(1f, 3f).SetRelative().SetEase(Ease.OutQuad))
                .Play();
        }

        public async UniTask ReturnPosition()
        {
            await DOTween.Sequence()
                .AppendInterval(1f)
                .Append(arm.DOMoveZ(armInitPosition.z, 3f))
                .Append(arm.DOMoveX(armInitPosition.x, 3f))
                .Play();
        }
    }
}
