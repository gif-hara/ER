using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>とアニメーションの仲介を行うクラス
    /// </summary>
    public sealed class ActorAnimationMediator : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;

        /// <summary>
        /// 移動速度の係数
        /// </summary>
        public float moveSpeedRate = 1.0f;

        /// <summary>
        /// 無敵
        /// </summary>
        public bool invisible = false;

        /// <summary>
        /// 先行入力が出来るか
        /// </summary>
        public bool advancedEntry = false;

        public void UseRecoveryItem()
        {
#if UNITY_EDITOR
            Debug.Log("UseRecoveryItem");
            return;
#endif
        }
    }
}
