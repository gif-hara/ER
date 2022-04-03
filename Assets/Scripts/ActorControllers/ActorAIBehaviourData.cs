using ER.ActorControllers;
using ER.EquipmentSystems;
using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="ActorAIController"/>が利用する<see cref="IBehaviourData"/>
    /// </summary>
    public sealed class ActorAIBehaviourData : IBehaviourData, IActorHolder
    {
        public Actor Actor { get; set; }

        public ActorAIController AIController { get; set; }
    }
}
