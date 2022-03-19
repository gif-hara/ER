using ER.ActorControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class DamageCalculator
    {
        public static int Calculate(ActorStatusData attackerData, ActorStatusData defenserData, float rate)
        {
            return
                CalculateInternal(attackerData, defenserData, rate, AttackAttributeType.Physics)
                + CalculateInternal(attackerData, defenserData, rate, AttackAttributeType.Magic)
                + CalculateInternal(attackerData, defenserData, rate, AttackAttributeType.Fire)
                + CalculateInternal(attackerData, defenserData, rate, AttackAttributeType.Earth)
                + CalculateInternal(attackerData, defenserData, rate, AttackAttributeType.Thunder)
                + CalculateInternal(attackerData, defenserData, rate, AttackAttributeType.Water)
                + CalculateInternal(attackerData, defenserData, rate, AttackAttributeType.Holy)
                + CalculateInternal(attackerData, defenserData, rate, AttackAttributeType.Dark);
        }

        private static int CalculateInternal(
            ActorStatusData attackerData,
            ActorStatusData defenserData,
            float rate,
            AttackAttributeType attackAttributeType
            )
        {
            var attack = attackerData.GetAttack(attackAttributeType);
            var defense = defenserData.GetDefense(attackAttributeType);
            var cutRate = defenserData.GetCutRate(attackAttributeType);

            defense = defense == 0 ? 1 : defense;
            cutRate = cutRate > 1.0f ? 1.0f : cutRate;

            return Mathf.FloorToInt(((attack * attack * rate) / defense) * (1.0f - cutRate));
        }
    }
}
