using System;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class CanNextAction : ITrigger
    {
        public bool Evaluate(IBehaviourData data)
        {
            return data.Cast<IActorHolder>().Actor.AnimationParameter.advancedEntry;
        }
    }
}
