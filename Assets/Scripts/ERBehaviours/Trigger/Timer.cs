using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class Timer : ITrigger
    {
        [SerializeField]
        private float delaySeconds = default;

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Timer(TimeSpan.FromSeconds(this.delaySeconds))
                .AsUnitObservable();
        }
    }
}
