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
    public sealed class OnBeginLeftEquipment : ITrigger
    {
        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var actorHolder = data.Cast<IActorHolder>();

                return actorHolder.Actor.Event.OnBeginLeftEquipmentSubject();
            });
        }
    }
}
