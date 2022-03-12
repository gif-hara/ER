using ER.ActorControllers;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LookAtActor : IAction
    {
        [SerializeField]
        private ActorType actorType = default;

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var actorHolder = data.Cast<IActorHolder>();
                var actor = actorHolder.Actor;
                actor.MotionController.BeginLookAt(ActorUtility.GetNearActor(this.actorType, actor.transform.position).transform);

                return Observable.ReturnUnit();
            });
        }
    }
}
