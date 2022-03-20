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
    public sealed class CanAdvancedEntry : ITrigger
    {
        public bool Evaluate(IBehaviourData data)
        {
            return this.Evaluate(data.Cast<IActorHolder>().Actor);
        }

        private bool Evaluate(Actor actor)
        {
            return actor.AnimationParameter.advancedEntry;
        }
    }
}
