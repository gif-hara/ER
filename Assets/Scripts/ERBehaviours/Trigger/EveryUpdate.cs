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
    public sealed class EveryUpdate : ITrigger
    {
        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                return Observable
                .EveryGameObjectUpdate()
                .AsUnitObservable();
            });
        }
    }
}
