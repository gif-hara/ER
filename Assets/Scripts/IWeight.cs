using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 重みを持つインターフェイス
    /// </summary>
    public interface IWeight
    {
        int Weight { get; }
    }
}
