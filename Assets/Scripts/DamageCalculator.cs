using ER.ActorControllers;
using ER.EquipmentSystems;
using ER.MasterDataSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class DamageCalculator
    {
        public static int Calculate(
            IActor attacker,
            EquipmentController attackerWeapon,
            IActor defenser,
            float rate
            )
        {
            return
                CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Physics)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Magic)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Fire)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Earth)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Thunder)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Water)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Holy)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Dark);
        }

        private static int CalculateInternal(
            IActor attacker,
            EquipmentController attackerWeapon,
            IActor defenser,
            float rate,
            AttackAttributeType attackAttributeType
            )
        {
            var weaponData = (WeaponInstanceData)attackerWeapon.EquipmentData;
            var attack =
                attacker.StatusController.BaseStatus.GetAttack(attackAttributeType)
                + weaponData.GetAttack(attackAttributeType);
            var defense = defenser.StatusController.BaseStatus.GetDefense(attackAttributeType);
            var cutRate = defenser.StatusController.BaseStatus.GetCutRate(attackAttributeType);

            defense = defense == 0 ? 1 : defense;

            if(defenser.EquipmentController.GuardingEquipmentController != null)
            {
                var guardingEquipmentController = defenser.EquipmentController.GuardingEquipmentController;
                var diff = (attacker.transform.position - defenser.transform.position).normalized;
                const float threshold = 90.0f;
                var angle = Vector2.Angle(defenser.transform.up, diff);
                if(angle <= threshold)
                {
                    var masterDataShield = guardingEquipmentController.EquipmentData as MasterDataShield.Record;
                    Assert.IsNotNull(masterDataShield, $"{guardingEquipmentController.name}に{typeof(MasterDataShield)}のデータがありません");

                    cutRate += masterDataShield.GetCutRate(attackAttributeType);
                }
            }
            
            cutRate = cutRate > 1.0f ? 1.0f : cutRate;

            var result = Mathf.FloorToInt(((attack * attack * rate) / defense) * (1.0f - cutRate));

            return result;
        }
    }
}
