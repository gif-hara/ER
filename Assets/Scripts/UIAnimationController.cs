using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIAnimationController : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector director = default;

        [SerializeField]
        private PlayableAsset inAsset = default;

        [SerializeField]
        private PlayableAsset outAsset = default;


        public void Play(bool isIn)
        {
            this.PlayInternalAsync(isIn, 0.0f)
                .Subscribe();
        }

        public void PlayImmediate(bool isIn)
        {
            this.PlayInternalAsync(isIn, 1.0f)
                .Subscribe();
        }

        public IObservable<Unit> PlayAsync(bool isIn)
        {
            return this.PlayInternalAsync(isIn, 0.0f);
        }

        private IObservable<Unit> PlayInternalAsync(bool isIn, float normalizedTime)
        {
            if (isIn)
            {
                this.director.playableAsset = this.inAsset;
            }
            else
            {
                this.director.playableAsset = this.outAsset;
            }

            this.director.extrapolationMode = DirectorWrapMode.None;
            this.director.initialTime = this.director.duration * normalizedTime;
            this.director.Play();

            return this.director.OnStoppedAsObservable();
        }
    }
}
