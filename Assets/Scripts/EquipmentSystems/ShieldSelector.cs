using ER.ActorControllers;
using ER.MasterDataSystem;
using I2.Loc;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.EquipmentSystems
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class ShieldSelector : IEquipmentSelector
    {
        [SerializeField, TermsPopup("Shield/")]
        private string shieldDataId = default;

        public void Attach(Actor actor, int index)
        {
            var item = actor.InventoryController.AddEquipment(this.shieldDataId, false);
            var masterDataShield = MasterDataShield.Get(item.ItemId);
            actor.EquipmentController.LeftHand.Attach(index, masterDataShield.EquipmentControllerPrefab, item.InstanceId);
        }
    }
}
