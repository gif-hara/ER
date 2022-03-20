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
        public bool Evalute(IBehaviourData data)
        {
            var actorHolder = data.Cast<IActorHolder>();
            var actor = actorHolder.Actor;

            return actor.EquipmentController.IsLeftRequest;
        }
    }
}
