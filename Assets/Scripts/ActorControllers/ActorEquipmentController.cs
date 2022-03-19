using ER.EquipmentSystems;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorEquipmentController
    {
        private Actor actor;

        private EquipmentController rightEquipemntContoller;

        private EquipmentController leftEquipmentController;

        public EquipmentController GuardingEquipmentController { get; private set; }

        public void Setup(Actor actor)
        {
            this.actor = actor;
        }

        public void AttachRightEquipment(EquipmentController equipmentPrefab, IEquipmentData equipmentData)
        {
            this.rightEquipemntContoller = equipmentPrefab.Attach(this.actor, equipmentData);
        }

        public void AttachLeftEquipment(EquipmentController equipmentPrefab, IEquipmentData equipmentData)
        {
            this.leftEquipmentController = equipmentPrefab.Attach(this.actor, equipmentData);
        }

        public void BeginGuard(EquipmentController equipmentController)
        {
            this.GuardingEquipmentController = equipmentController;
            Debug.Log("BeginGuard");
        }

        public void EndGuard()
        {
            this.GuardingEquipmentController = null;
            Debug.Log("EndGuard");
        }

        public EquipmentController GetEquipmentController(HandType handType)
        {
            switch (handType)
            {
                case HandType.Left:
                    return this.leftEquipmentController;
                case HandType.Right:
                    return this.rightEquipemntContoller;
                default:
                    Assert.IsTrue(false, $"{handType}は未実装です");
                    return null;
            }
        }
    }
}
