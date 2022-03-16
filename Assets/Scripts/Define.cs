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
}
