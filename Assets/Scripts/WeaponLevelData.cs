using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class WeaponLevelData
    {
        private const int Min = 0;

        private const int Max = 99;

        [SerializeField, Range(Min, Max)]
        private int physicsLevel = default;

        [SerializeField, Range(Min, Max)]
        private int magicLevel = default;

        [SerializeField, Range(Min, Max)]
        private int fireLevel = default;

        [SerializeField, Range(Min, Max)]
        private int earthLevel = default;

        [SerializeField, Range(Min, Max)]
        private int thunderLevel = default;

        [SerializeField, Range(Min, Max)]
        private int waterLevel = default;

        [SerializeField, Range(Min, Max)]
        private int holyLevel = default;

        [SerializeField, Range(Min, Max)]
        private int darkLevel = default;

        public float GetRate(AttackAttributeType attackAttributeType)
        {
            return (float)GetLevel(attackAttributeType) / Max;
        }

        public int GetLevel(AttackAttributeType attackAttributeType)
        {
            switch (attackAttributeType)
            {
                case AttackAttributeType.Physics:
                    return this.physicsLevel;
                case AttackAttributeType.Magic:
                    return this.magicLevel;
                case AttackAttributeType.Fire:
                    return this.fireLevel;
                case AttackAttributeType.Earth:
                    return this.earthLevel;
                case AttackAttributeType.Thunder:
                    return this.thunderLevel;
                case AttackAttributeType.Water:
                    return this.waterLevel;
                case AttackAttributeType.Holy:
                    return this.holyLevel;
                case AttackAttributeType.Dark:
                    return this.darkLevel;
                default:
                    Assert.IsTrue(false, $"{attackAttributeType}は未対応です");
                    return 0;
            }
        }
    }
}
