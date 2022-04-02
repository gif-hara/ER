using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// <see cref="IProbability"/>に関する拡張関数
    /// </summary>
    public static class IProbabilityExtensions
    {
        /// <summary>
        /// 抽選を行う
        /// </summary>
        public static bool Lottery(this IProbability self)
        {
            return Random.value < self.Probability;
        }
    }
}
