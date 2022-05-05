using ER.ActorControllers;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 攻撃を開始する
    /// </summary>
    public sealed class BeginAttack : IAction
    {
        [SerializeField]
        private AnimationClip attackClip = default;

        /// <summary>
        /// ダメージ係数
        /// </summary>
        [SerializeField]
        private float power = 1.0f;

        /// <summary>
        /// ノックバック蓄積値の係数
        /// </summary>
        [SerializeField]
        private float knockBackAccumulate = 1.0f;

        /// <summary>
        /// ノックバックの威力
        /// </summary>
        [SerializeField]
        private float knockBackPower = default;
        
        [SerializeField]
        private HandType handType = HandType.Right;

        public void Invoke(IBehaviourData data)
        {
            var behaviourData = data.Cast<IActorHolder>();
            var equipmentController = behaviourData.Actor.EquipmentController.GetEquipmentController(this.handType);
            var actor = behaviourData.Actor;

            actor.EquipmentController.BeginAttack(equipmentController);

            actor.AnimationController.PlayOneShotAsync(this.attackClip)
                 .Subscribe(_ =>
                            {
                                actor.EquipmentController.EndAttack();
                                actor.StateController.ChangeRequest(ActorStateController.StateType.Movable);
                            })
                 .AddTo(equipmentController);

            equipmentController.Power = this.power;
            equipmentController.KnockBackAccumulate = this.knockBackAccumulate;
            equipmentController.KnockbBackPower = this.knockBackPower;
            actor.StateController.ChangeRequest(ActorStateController.StateType.Attack);
        }
    }
}