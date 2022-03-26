using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

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

        private void Awake()
        {
            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                .Subscribe(x =>
                {
                    if (x.NextState == ActorStateController.StateType.Movable)
                    {
                        actor.AnimationParameter.moveSpeedRate = 1.0f;
                        actor.AnimationParameter.invisible = false;
                        actor.AnimationParameter.advancedEntry = true;
                    }

                    if (x.NextState == ActorStateController.StateType.Guard)
                    {
                        actor.AnimationParameter.moveSpeedRate = 0.5f;
                        actor.AnimationParameter.invisible = false;
                        actor.AnimationParameter.advancedEntry = true;
                    }

                })
                .AddTo(actor.Disposables);
        }

        public void UseRecoveryItem()
        {
#if UNITY_EDITOR
            Debug.Log("UseRecoveryItem");
            return;
#endif
        }
    }
}
