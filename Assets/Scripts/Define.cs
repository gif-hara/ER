using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 手のタイプ
    /// </summary>
    public enum HandType
    {
        Left,
        Right,
    }

    public enum ActorType
    {
        All,
        Player,
        Enemy,
    }

    public enum ItemCategory
    {
        Unknown,
        Weapon,
        ArmorHead,
        ArmorTorso,
        ArmorArm,
        ArmorLeg,
        Accessory,
        Valuable,
        Shield,
    }

    public enum EquipmentGrowthType
    {
        Linear,
        OutQuad,
        OutCubic,
        InQuad,
        InCubic,
    }

    public enum AttackAttributeType
    {
        Physics,
        Magic,
        Fire,
        Earth,
        Thunder,
        Water,
        Holy,
        Dark,
    }

    public enum ArmorType
    {
        Head,
        Torso,
        Arm,
        Leg
    }

    /// <summary>
    /// インゲーム中のメニュータイプ
    /// </summary>
    public enum IngameMenuType
    {
        /// <summary>
        /// スタートボタンが押された際のメニュー
        /// </summary>
        FromStartButton,

        /// <summary>
        /// チェックポイントメニュー
        /// </summary>
        CheckPoint,
    }

    public static class Define
    {
        /// <summary>
        /// 手に装備可能な装備品の数
        /// </summary>
        public const int EquipmentableNumber = 3;
    }
}
