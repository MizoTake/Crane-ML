using DG.Tweening;
using Doozy.Engine.UI;
using UniRx;
using UnityEngine;

namespace CranML
{
    public class CraneGamePresenter : MonoBehaviour
    {

        [SerializeField] private UIButton leftButton;
        [SerializeField] private UIButton forwardButton;
        [SerializeField] private Transform arm;
        [SerializeField] private Animator armAnim;
        
        void Start()
        {
            Bind();
        }

        private void Bind()
        {
            // TODO: 初期値からの移動場所
            
            leftButton.Button.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    arm.DOMoveX(arm.transform.position.x + 0.01f, 0.1f);
                });
            
            forwardButton.Button.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    arm.DOMoveZ(arm.transform.position.z + 0.01f, 0.1f);
                });
        }
    }
}
