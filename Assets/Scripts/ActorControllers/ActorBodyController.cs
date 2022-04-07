using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>のボディ（ゲームオブジェクト）部分を制御するクラス
    /// </summary>
    public sealed class ActorBodyController : MonoBehaviour
    {
        /// <summary>
        /// モデルの親オブジェクト
        /// </summary>
        [SerializeField]
        private Transform modelParent = default;

        public Transform ModelParent => this.modelParent;
    }
}
