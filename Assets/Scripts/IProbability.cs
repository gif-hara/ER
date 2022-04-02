using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 確率を持つインターフェイス
    /// </summary>
    public interface IProbability
    {
        /// <summary>
        /// 確率
        /// </summary>
        float Probability { get; }
    }
}
