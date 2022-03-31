using System;
using UnityEngine;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class IsMatchNextAttackIndex : ITrigger
    {
        [SerializeField]
        private int targetNextAttackIndex = default;

        public bool Evaluate(IBehaviourData data)
        {
            return data.Cast<IActorHolder>().Actor.AnimationParameter.nextAttackIndex == this.targetNextAttackIndex;
        }
    }
}
