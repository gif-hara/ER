using ER.EquipmentSystems;
using ER.ERBehaviour;
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

        /// <summary>
        /// 次の攻撃インデックス
        /// </summary>
        public int nextAttackIndex = 0;

        private void Awake()
        {
            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                 .Subscribe(x =>
                 {
                     switch (x.NextState)
                     {
                         case ActorStateController.StateType.Movable:
                             moveSpeedRate = 1.0f;
                             invisible = false;
                             advancedEntry = true;
                             nextAttackIndex = 0;
                             break;
                         case ActorStateController.StateType.Guard:
                             moveSpeedRate = 0.5f;
                             invisible = false;
                             advancedEntry = true;
                             nextAttackIndex = 0;
                             break;
                         case ActorStateController.StateType.UseRecoveryItem:
                             moveSpeedRate = 0.5f;
                             invisible = false;
                             break;
                         case ActorStateController.StateType.Attack:
                             this.advancedEntry = false;
                             break;
                         case ActorStateController.StateType.KnockBack:
                             moveSpeedRate = 0.0f;
                             invisible = false;
                             advancedEntry = false;
                             break;
                     }
                 })
                 .AddTo(actor.Disposables);
        }

        /// <summary>
        /// 回復アイテムを使用する
        /// </summary>
        public void UseRecoveryItem()
        {
#if UNITY_EDITOR
            Debug.Log("UseRecoveryItem");
#endif
            actor.StatusController.UseRecoveryItem();
        }

        /// <summary>
        /// 攻撃中の武器のコライダーオブジェクトの有効性を設定する
        /// </summary>
        public void SetActiveEquipmentCollider(SetActiveEquipmentColliderData data)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            actor
                .EquipmentController
                .GetHand(data.handType)
                .CurrentEquipmentController
                .SetupCollider(data.isActive, data.power, data.knockBackAccumulate);
        }

        public void CanBeNextAction()
        {
            this.advancedEntry = true;
        }

        public void SetNextAttackIndex(int index)
        {
            this.nextAttackIndex = index;
        }


        public void Test(object a)
        {
            
        }
    }
}