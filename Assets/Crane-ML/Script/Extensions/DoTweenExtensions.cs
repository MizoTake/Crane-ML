using DG.Tweening;
using UnityEngine;

namespace CraneML.Extensions
{
    public static class DoTweenExtensions
    {
        public struct TweenAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion
        {
            Tween tween;

            public TweenAwaiter(Tween tween) => this.tween = tween;

            public bool IsCompleted => tween.IsComplete();

            public void GetResult() { }

            public void OnCompleted(System.Action continuation) => tween.OnKill(() => continuation());

            public void UnsafeOnCompleted(System.Action continuation) => tween.OnKill(() => continuation());
        }

        public static TweenAwaiter GetAwaiter(this Tween self)
        {
            return new TweenAwaiter(self);
        }
    }
}
