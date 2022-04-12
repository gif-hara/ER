using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

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
    }
}
