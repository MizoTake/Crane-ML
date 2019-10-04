using System;
using System.Threading;
using Doozy.Engine.UI;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace CranML
{
    public class CraneGameView : MonoBehaviour
    {
        
        [SerializeField] private Transform arm;
        [SerializeField] private UIButton horizontalUIButton;
        [SerializeField] private UIButton verticalUIButton;
        
        public Button HorizontalButton => horizontalUIButton.Button;
        public Button VerticalButton => verticalUIButton.Button;

        public bool HorizontalButtonUp { get; set; }
        public bool VerticalButtonUp { get; set; }

        // TODO: 初期値からの移動場所
        // TODO: 移動制限(Clampとかで)

        public async UniTask Move(Vector3 moveVector, CancellationToken token)
        {
            while(!HorizontalButtonUp || !VerticalButtonUp)
            {
                arm.transform.Translate(moveVector);
                await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime), cancellationToken: token);
            }
        }
    }
}
