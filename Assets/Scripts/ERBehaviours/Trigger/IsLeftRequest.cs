using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class IsLeftRequest : ITrigger
    {
        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var actorHolder = data.Cast<IActorHolder>();
                var actor = actorHolder.Actor;
                return actor.UpdateAsObservable()
                .Where(_ => actor.EquipmentController.IsLeftRequest)
                .AsUnitObservable();
            });
        }
    }
}
