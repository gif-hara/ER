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

        public void Attach(Actor actor)
        {
            var masterDataShield = MasterDataShield.Get(this.shieldDataId);
            actor.EquipmentController.LeftHand.Attach(masterDataShield.EquipmentControllerPrefab, masterDataShield);
        }
    }
}
