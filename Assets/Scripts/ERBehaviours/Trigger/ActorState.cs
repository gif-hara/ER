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

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var actorHolder = data.Cast<IActorHolder>();
                var actor = actorHolder.Actor;
                return actor.StateController.ObserveEveryValueChanged(x => x.CurrentState)
                .DistinctUntilChanged()
                .Where(x => x == this.stateType)
                .AsUnitObservable();
            });
        }
    }
}
