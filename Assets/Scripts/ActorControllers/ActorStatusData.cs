using I2.Loc;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class ActorStatusData
    {
        [TermsPopup]
        public string name = default;

        public int hitPoint = default;

        public int physicsAttack = default;

        public int magicAttack = default;

        public int fireAttack = default;

        public int earthAttack = default;

        public int thunderAttack = default;

        public int waterAttack = default;

        public int holyAttack = default;

        public int darkAttack = default;

        public int physicsDefense = default;

        public int magicDefense = default;

        public int fireDefense = default;

        public int earthDefense = default;

        public int thunderDefense = default;

        public int waterDefense = default;

        public int holyDefense = default;

        public int darkDefense = default;

        [Range(0.0f, 1.0f)]
        public float physicsCutRate = default;

        [Range(0.0f, 1.0f)]
        public float magicCutRate = default;

        [Range(0.0f, 1.0f)]
        public float fireCutRate = default;

        [Range(0.0f, 1.0f)]
        public float earthCutRate = default;

        [Range(0.0f, 1.0f)]
        public float thunderCutRate = default;

        [Range(0.0f, 1.0f)]
        public float waterCutRate = default;

        [Range(0.0f, 1.0f)]
        public float holyCutRate = default;

        [Range(0.0f, 1.0f)]
        public float darkCutRate = default;

        public string LocalizedName => LocalizationManager.GetTermTranslation(this.name);

        public int GetAttack(AttackAttributeType type)
        {
            switch (type)
            {
                case AttackAttributeType.Physics:
                    return this.physicsAttack;
                case AttackAttributeType.Magic:
                    return this.magicAttack;
                case AttackAttributeType.Fire:
                    return this.fireAttack;
                case AttackAttributeType.Earth:
                    return this.earthAttack;
                case AttackAttributeType.Thunder:
                    return this.thunderAttack;
                case AttackAttributeType.Water:
                    return this.waterAttack;
                case AttackAttributeType.Holy:
                    return this.holyAttack;
                case AttackAttributeType.Dark:
                    return this.darkAttack;
                default:
                    Assert.IsTrue(false, $"{type}は未対応です");
                    return 0;
            }
        }

        public int GetDefense(AttackAttributeType type)
        {
            switch (type)
            {
                case AttackAttributeType.Physics:
                    return this.physicsDefense;
                case AttackAttributeType.Magic:
                    return this.magicDefense;
                case AttackAttributeType.Fire:
                    return this.fireDefense;
                case AttackAttributeType.Earth:
                    return this.earthDefense;
                case AttackAttributeType.Thunder:
                    return this.thunderDefense;
                case AttackAttributeType.Water:
                    return this.waterDefense;
                case AttackAttributeType.Holy:
                    return this.holyDefense;
                case AttackAttributeType.Dark:
                    return this.darkDefense;
                default:
                    Assert.IsTrue(false, $"{type}は未対応です");
                    return 0;
            }
        }

        public float GetCutRate(AttackAttributeType type)
        {
            switch (type)
            {
                case AttackAttributeType.Physics:
                    return this.physicsCutRate;
                case AttackAttributeType.Magic:
                    return this.magicCutRate;
                case AttackAttributeType.Fire:
                    return this.fireCutRate;
                case AttackAttributeType.Earth:
                    return this.earthCutRate;
                case AttackAttributeType.Thunder:
                    return this.thunderCutRate;
                case AttackAttributeType.Water:
                    return this.waterCutRate;
                case AttackAttributeType.Holy:
                    return this.holyCutRate;
                case AttackAttributeType.Dark:
                    return this.darkCutRate;
                default:
                    Assert.IsTrue(false, $"{type}は未対応です");
                    return 0.0f;
            }
        }
    }
}
