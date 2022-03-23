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

        public void Attach(Actor actor, int index)
        {
            var item = actor.InventoryController.AddEquipment(this.weaponDataId);
            var masterDataWeapon = MasterDataWeapon.Get(item.ItemId);
            actor.EquipmentController.RightHand.Attach(index, masterDataWeapon.EquipmentControllerPrefab, item.InstanceId);
        }
    }
}
