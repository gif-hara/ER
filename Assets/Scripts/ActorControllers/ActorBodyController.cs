using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>のボディ（ゲームオブジェクト）部分を制御するクラス
    /// </summary>
    public sealed class ActorBodyController : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;
        
        /// <summary>
        /// モデルの親オブジェクト
        /// </summary>
        [SerializeField]
        private Transform modelParent = default;

        /// <summary>
        /// 左手の親オブジェクト
        /// </summary>
        [SerializeField]
        private Transform leftHandParent = default;

        /// <summary>
        /// 右手の親オブジェクト
        /// </summary>
        [SerializeField]
        private Transform rightHandParent = default;

        public Transform ModelParent => this.modelParent;

        void Awake()
        {
            this.actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                .Subscribe(x =>
                {
                    switch (x.NextState)
                    {
                        case ActorStateController.StateType.Movable:
                            this.ModelParent.localRotation = Quaternion.identity;
                            break;
                    }
                })
                .AddTo(this);
        }
        
        public Transform GetHandParent(HandType handType)
        {
            switch (handType)
            {
                case HandType.Left:
                    return this.leftHandParent;
                case HandType.Right:
                    return this.rightHandParent;
                default:
                    Assert.IsTrue(false, $"{handType}は未対応です");
                    return null;
            }
        }
    }
}
