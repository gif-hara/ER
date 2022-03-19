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

        [SerializeField]
        private WeaponLevelData levelData = default;

        public void AttachRight(Actor actor)
        {
            var masterDataWeapon = MasterDataWeapon.Get(this.weaponDataId);
            var instanceData = new WeaponInstanceData(masterDataWeapon, levelData);
            actor.EquipmentController.SetRightEquipment(masterDataWeapon.EquipmentControllerPrefab, instanceData);
        }
    }
}
