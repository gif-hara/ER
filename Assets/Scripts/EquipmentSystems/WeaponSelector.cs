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
        private WeaponLevelData levelData = default;

        public void Attach(Actor actor)
        {
            var masterDataWeapon = MasterDataWeapon.Get(this.weaponDataId);
            var instanceData = new WeaponInstanceData(masterDataWeapon, levelData);
            actor.EquipmentController.RightHand.Attach(masterDataWeapon.EquipmentControllerPrefab, instanceData);
        }
    }
}
