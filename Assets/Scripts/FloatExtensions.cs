using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// パーセンテージ形式にして返す
        /// </summary>
        public static int ToPercentage(this float self)
        {
            return Mathf.FloorToInt(self * 100);
        }
    }
}
