using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PlayTimeline : IAction
    {
        [SerializeField]
        private PlayableAsset playableAsset = default;

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                return Observable.ReturnUnit();
            });
        }
    }
}
