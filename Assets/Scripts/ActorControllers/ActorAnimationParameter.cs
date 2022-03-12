using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>のアニメーションに関するパラメータを持つクラス
    /// </summary>
    public sealed class ActorAnimationParameter : MonoBehaviour
    {
        /// <summary>
        /// 移動速度の係数
        /// </summary>
        public float moveSpeedRate = 1.0f;

        /// <summary>
        /// 無敵
        /// </summary>
        public bool invisible = false;
    }
}
