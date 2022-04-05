using System;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FadeUIView : UIView
    {
        [SerializeField]
        private PlayableDirector director = default;
        
        [SerializeField]
        private PlayableAsset inAnimation = default;

        [SerializeField]
        private PlayableAsset outAnimation = default;

        public IObservable<Unit> PlayInAsync()
        {
            return this.PlayAsyncInternal(this.inAnimation);
        }

        public IObservable<Unit> PlayOutAsync()
        {
            return this.PlayAsyncInternal(this.outAnimation);
        }

        public void PlayInImmediate()
        {
            this.PlayImmediateInternal(this.inAnimation);
        }

        public void PlayOutImmediate()
        {
            this.PlayImmediateInternal(this.outAnimation);
        }
        
        private IObservable<Unit> PlayAsyncInternal(PlayableAsset asset)
        {
            return Observable.Defer(() =>
            {
                this.director.initialTime = 0.0f;
                this.director.Play(asset);
                return this.director.OnStoppedAsObservable();
            });
        }

        private void PlayImmediateInternal(PlayableAsset asset)
        {
            this.director.playableAsset = asset;
            this.director.initialTime = this.director.duration;
            this.director.Stop();
            this.director.Evaluate();
        }
    }
}
