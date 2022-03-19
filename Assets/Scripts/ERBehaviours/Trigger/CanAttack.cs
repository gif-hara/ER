using ER.ActorControllers;
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
    public sealed class CanAttack : ITrigger
    {
        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var actor = data.Cast<IActorHolder>().Actor;
                return actor.gameObject
                .UpdateAsObservable()
                .Where(_ => Evalute(actor));
            });
        }

        private bool Evalute(Actor actor)
        {
            var currentState = actor.StateController.CurrentState;

            switch (currentState)
            {
                case ActorStateController.StateType.Movable:
                case ActorStateController.StateType.Guard:
                    return true;
                case ActorStateController.StateType.Attack:
                case ActorStateController.StateType.Avoidance:
                    return actor.AnimationParameter.advancedEntry;
                default:
                    Debug.LogWarning($"{currentState}に対応しましょう");
                    return false;
            }
        }
    }
}
