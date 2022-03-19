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

        public void Setup(Actor actor)
        {
            this.actor = actor;
        }

        public void SetRightEquipment(EquipmentController equipmentPrefab, IEquipmentData equipmentData)
        {
            this.rightEquipemntContoller = equipmentPrefab.Attach(this.actor, equipmentData);
        }

        public void AttachLeftEquipment(EquipmentController equipmentPrefab, IEquipmentData equipmentData)
        {
            this.leftEquipmentController = equipmentPrefab.Attach(this.actor, equipmentData);
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
