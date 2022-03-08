using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EquipmentBehaviourData : IBehaviourData, IActorHolder
    {
        public IActor Actor { get; set; }
    }
}
