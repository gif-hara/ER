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
        [SerializeField, TermsPopup("Item/")]
        private string weaponDataId = default;

        public void AttachRight(Actor actor)
        {
            var weaponData = MasterDataWeapon.Get(this.weaponDataId);
            actor.EquipmentController.SetRightEquipment(weaponData.EquipmentControllerPrefab, weaponData);
        }
    }
}
