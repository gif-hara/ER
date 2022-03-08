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
    public sealed class OnBeginRightEquipment : ITrigger
    {
        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            var actorHolder = data as IActorHolder;
            Assert.IsNotNull(actorHolder);

            return actorHolder.Actor.OnBeginRightEquipmentAsObservable();
        }
    }
}
