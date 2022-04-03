using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>の<see cref="PlayableDirector"/>を制御するクラス
    /// </summary>
    public sealed class ActorDirectorController
    {
        private IActor actor;

        private PlayableDirector director;

        /// <summary>
        /// 利用可能な状態にする
        /// </summary>
        public void Setup(IActor actor, PlayableDirector director)
        {
            this.actor = actor;
            this.director = director;
        }

        /// <summary>
        /// <paramref name="asset"/>を一度だけ再生する
        /// </summary>
        public IObservable<Unit> PlayOneShotAsync(PlayableAsset asset)
        {
            return Observable.Defer(() =>
            {
                this.director.playableAsset = asset;
                this.director.SetGenericBinding("ActorAnimation", this.actor.Animator);
                this.director.Play();

                return this.director.OnStoppedAsObservable();
            });
        }
    }
}
