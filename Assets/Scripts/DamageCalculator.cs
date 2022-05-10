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
            var result =
                CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Physics)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Magic)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Fire)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Earth)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Thunder)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Water)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Holy)
                + CalculateInternal(attacker, attackerWeapon, defenser, rate, AttackAttributeType.Dark);
            return result;
        }

        private static int CalculateInternal(
            IActor attacker,
            EquipmentController attackerWeapon,
            IActor defenser,
            float rate,
            AttackAttributeType attackAttributeType
            )
        {
            const int ValuableItemRate = 2;
            
            var weaponItem = attackerWeapon.Item;
            var masterDataWeapon = weaponItem.MasterDataItem.ToWeapon();
            var weaponLevelData = attacker.InventoryController.WeaponLevelDatabase[weaponItem.InstanceId];
            var weaponAttack = masterDataWeapon.GetAttackElement(attackAttributeType).Evaluate(weaponLevelData.GetRate(attackAttributeType));
            var attack =
                attacker.StatusController.BaseStatus.GetAttack(attackAttributeType)
                + attacker.InventoryController.GetAttackValuableNumber(attackAttributeType) * ValuableItemRate
                + weaponAttack;
            var defense =
                defenser.StatusController.BaseStatus.GetDefense(attackAttributeType)
                + defenser.EquipmentController.GetDefense(attackAttributeType)
                + defenser.InventoryController.GetDefenseValuableNumber(attackAttributeType) * ValuableItemRate;
            var cutRate = defenser.StatusController.BaseStatus.GetCutRate(attackAttributeType);

            defense = defense == 0 ? 1 : defense;

            if (defenser.EquipmentController.GuardingEquipmentController != null)
            {
                var guardingEquipmentController = defenser.EquipmentController.GuardingEquipmentController;
                var diff = (attacker.transform.position - defenser.transform.position).normalized;
                const float threshold = 90.0f;
                var angle = Vector2.Angle(defenser.transform.up, diff);
                if (angle <= threshold)
                {
                    var masterDataShield = guardingEquipmentController.Item.MasterDataItem.ToShield();
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
