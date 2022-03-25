using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        public static bool IsEquipment(this ItemCategory self)
        {
            switch (self)
            {
                case ItemCategory.Weapon:
                case ItemCategory.ArmorHead:
                case ItemCategory.ArmorTorso:
                case ItemCategory.ArmorArm:
                case ItemCategory.ArmorLeg:
                case ItemCategory.Accessory:
                case ItemCategory.Shield:
                    return true;
                case ItemCategory.Valuable:
                    return false;
                default:
                    Assert.IsTrue(false, $"{self}は未対応です");
                    return false;
            }
        }

        public static ItemCategory ToItemCategory(this ArmorType self)
        {
            switch (self)
            {
                case ArmorType.Head:
                    return ItemCategory.ArmorHead;
                case ArmorType.Torso:
                    return ItemCategory.ArmorTorso;
                case ArmorType.Arm:
                    return ItemCategory.ArmorArm;
                case ArmorType.Leg:
                    return ItemCategory.ArmorLeg;
                default:
                    Assert.IsTrue(false, $"{self}は未対応です");
                    return ItemCategory.Unknown;
            }
        }
    }
}