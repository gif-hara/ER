using ER.ActorControllers;
using ER.EquipmentSystems;
using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorAIBehaviourData : IBehaviourData, IActorHolder
    {
        public IActor Actor { get; set; }

        public ActorAIController AIController { get; set; }
    }
}
