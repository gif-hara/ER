using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 貴重品アイテムに関するユーティリティクラス
    /// </summary>
    public static class ValuableItemUtility
    {
        /// <summary>
        /// 攻撃力上昇系貴重品のIdを返す
        /// </summary>
        public static string GetAttackId(AttackAttributeType attackAttributeType)
        {
            switch (attackAttributeType)
            {
                case AttackAttributeType.Physics:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010101)}";
                case AttackAttributeType.Magic:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010102)}";
                case AttackAttributeType.Fire:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010103)}";
                case AttackAttributeType.Earth:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010104)}";
                case AttackAttributeType.Thunder:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010105)}";
                case AttackAttributeType.Water:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010106)}";
                case AttackAttributeType.Holy:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010107)}";
                case AttackAttributeType.Dark:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010108)}";
                default:
                    Assert.IsTrue(false, $"{attackAttributeType}は未対応です");
                    return null;
            }
        }
        /// <summary>
        /// 防御力上昇系貴重品のIdを返す
        /// </summary>
        public static string GetDefenseId(AttackAttributeType attackAttributeType)
        {
            switch (attackAttributeType)
            {
                case AttackAttributeType.Physics:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010109)}";
                case AttackAttributeType.Magic:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010110)}";
                case AttackAttributeType.Fire:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010111)}";
                case AttackAttributeType.Earth:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010112)}";
                case AttackAttributeType.Thunder:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010113)}";
                case AttackAttributeType.Water:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010114)}";
                case AttackAttributeType.Holy:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010115)}";
                case AttackAttributeType.Dark:
                    return $"{nameof(ScriptLocalization.ValuableItem)}/{nameof(ScriptLocalization.ValuableItem.Name010116)}";
                default:
                    Assert.IsTrue(false, $"{attackAttributeType}は未対応です");
                    return null;
            }
        }
    }
}
