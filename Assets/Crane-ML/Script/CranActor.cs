using System;
using CraneML.Extensions;
using DG.Tweening;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using UnityEngine;

namespace CraneML
{
    public class CranActor : MonoBehaviour
    {

        [SerializeField] private Transform leftArm;
        [SerializeField] private Transform rightArm;
        [SerializeField] private Transform armCore;

        private Tween leftOpenAnim;
        private Tween rightOpenAnim;
        private Tween leftCatchAnim;
        private Tween rightCatchAnim;
        private Tween armDownAnim;

        private void Start()
        {
            leftOpenAnim = leftArm.DOLocalRotate(Vector3.right * 25, 0.5f).SetEase(Ease.InQuad).SetRelative();
            rightOpenAnim = rightArm.DOLocalRotate(Vector3.left * 25, 0.5f).SetEase(Ease.InQuad).SetRelative();
            leftCatchAnim = leftArm.DOLocalRotate(Vector3.left * 25, 0.5f).SetEase(Ease.InQuad).SetRelative();
            rightCatchAnim = rightArm.DOLocalRotate(Vector3.right * 25, 0.5f).SetEase(Ease.InQuad).SetRelative();
            armDownAnim = transform.DOMoveY(-1f, 3f).SetRelative().SetEase(Ease.InQuad);
            
            leftArm.OnCollisionEnterAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ => leftCatchAnim.Pause());

            rightArm.OnCollisionEnterAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ => rightCatchAnim.Pause());

            armCore.OnCollisionEnterAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ => armDownAnim.Pause());
        }

        public Tween Open()
        {
            return DOTween.Sequence()
                .Append(leftOpenAnim)
                .Join(rightOpenAnim);
        }

        public Tween Catch()
        {
            return DOTween.Sequence()
                .Append(leftCatchAnim)
                .Join(rightCatchAnim);
        }

        public Tween Down() => armDownAnim;
    }
}
