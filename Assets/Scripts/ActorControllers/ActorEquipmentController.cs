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

        public void SetRightEquipment(EquipmentController equipmentPrefab)
        {
            this.rightEquipemntContoller = equipmentPrefab.Attach(this.actor);
        }

        public EquipmentController GetEquipmentController(HandType handType)
        {
            switch (handType)
            {
                case HandType.Left:
                    throw new NotImplementedException();
                case HandType.Right:
                    return this.rightEquipemntContoller;
                default:
                    Assert.IsTrue(false, $"{handType}は未実装です");
                    return null;
            }
        }
    }
}
