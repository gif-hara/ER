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
    public sealed class WeaponSelector : IEquipmentSelector
    {
        [SerializeField, TermsPopup("Weapon/")]
        private string weaponDataId = default;

        [SerializeField]
        private HandType handType = HandType.Right;

        public void Attach(Actor actor, int index)
        {
            var item = actor.InventoryController.AddEquipment(this.weaponDataId, false);
            var masterDataWeapon = MasterDataWeapon.Get(item.ItemId);
            actor.EquipmentController.GetHand(this.handType).Attach(index, masterDataWeapon.EquipmentControllerPrefab, item.InstanceId);
        }
    }
}
