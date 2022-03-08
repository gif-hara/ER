using ER.EquipmentSystems;
using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EquipmentBehaviourData : IBehaviourData, IActorHolder, IEquipmentControllerHolder
    {
        public IActor Actor { get; set; }

        public EquipmentController EquipmentController { get; set; }
    }
}
