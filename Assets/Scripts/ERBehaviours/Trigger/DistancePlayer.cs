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
    public sealed class DistancePlayer : ITrigger
    {
        [SerializeField]
        private float distanceThreshold = default;

        /// <summary>
        /// <see cref="distanceThreshold"/>の中に入った場合に発火するか
        /// </summary>
        [SerializeField]
        private bool isIn = default;

        public bool Evalute(IBehaviourData data)
        {
            var actorHolder = data.Cast<IActorHolder>();
            var actor = actorHolder.Actor;

            foreach (var i in Actor.Players)
            {
                var distance = (actor.transform.position - i.transform.position).magnitude;
                if (this.IsSatisfy(distance))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSatisfy(float distance)
        {
            if (this.isIn)
            {
                return distance <= this.distanceThreshold;
            }
            else
            {
                return distance >= this.distanceThreshold;
            }
        }
    }
}
