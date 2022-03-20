using ER.ActorControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// <see cref="Actor"/>のステートが一致した場合に発火される<see cref="ITrigger"/>
    /// </summary>
    [Serializable]
    public sealed class ActorState : ITrigger
    {
        [SerializeField]
        private ActorStateController.StateType stateType = default;

        public bool Evaluate(IBehaviourData data)
        {
            var actorHolder = data.Cast<IActorHolder>();
            var actor = actorHolder.Actor;

            return this.stateType == actor.StateController.CurrentState;
        }
    }
}
