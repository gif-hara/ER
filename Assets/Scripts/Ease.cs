using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class Ease
    {
        public static float Evalute(float value, EquipmentGrowthType type)
        {
            switch (type)
            {
                case EquipmentGrowthType.Linear:
                    return Linear(value);
                case EquipmentGrowthType.OutQuad:
                    return OutQuad(value);
                case EquipmentGrowthType.OutCubic:
                    return OutCubic(value);
                case EquipmentGrowthType.InQuad:
                    return InQuad(value);
                case EquipmentGrowthType.InCubic:
                    return InCubic(value);
                default:
                    Assert.IsTrue(false, $"{type}は未対応です");
                    return 0.0f;
            }
        }

        public static float Linear(float value)
        {
            return Mathf.Clamp01(value);
        }

        public static float OutQuad(float value)
        {
            return 1.0f - (1.0f - value) * (1.0f - value);
        }

        public static float OutCubic(float value)
        {
            return 1.0f - (1.0f - value) * (1.0f - value) * (1.0f * value);
        }

        public static float InQuad(float value)
        {
            return value * value;
        }

        public static float InCubic(float value)
        {
            return value * value * value;
        }

    }
}
