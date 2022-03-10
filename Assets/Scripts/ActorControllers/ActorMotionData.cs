using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>の移動と回転に関するデータ
    /// </summary>
    [Serializable]
    public sealed class ActorMotionData
    {
        /// <summary>
        /// 移動速度
        /// </summary>
        public float moveSpeed = default;

        /// <summary>
        /// 衝突用の半径
        /// </summary>
        public float radius = default;
    }
}
