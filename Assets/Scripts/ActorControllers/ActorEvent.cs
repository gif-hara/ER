using ER.EquipmentSystems;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>に発生するイベントを持つクラス
    /// </summary>
    public sealed class ActorEvent : IDisposable
    {
        /// <summary>
        /// 右手装備品の使用を開始した際のイベント
        /// </summary>
        private readonly Subject<Unit> beginRightEquipmentSubject = new Subject<Unit>();

        /// <summary>
        /// 右手装備品の使用を完了した際のイベント
        /// </summary>
        private readonly Subject<Unit> endRightEquipmentSubject = new Subject<Unit>();

        /// <summary>
        /// 相手から攻撃を受けた際のイベント
        /// </summary>
        private readonly Subject<EquipmentController> onHitOpponentAttack = new Subject<EquipmentController>();

        /// <summary>
        /// 死亡した際のイベント
        /// </summary>
        private readonly Subject<Unit> onDead = new Subject<Unit>();

        /// <summary>
        /// ステートが切り替わった際のイベント
        /// </summary>
        private readonly Subject<ActorStateController.StateType> onChangedStateType = new Subject<ActorStateController.StateType>();

        /// <summary>
        /// <inheritdoc cref="beginRightEquipmentSubject"/>
        /// </summary>
        public ISubject<Unit> OnBeginRightEquipmentSubject() => this.beginRightEquipmentSubject;

        /// <summary>
        /// <inheritdoc cref="endRightEquipmentSubject"/>
        /// </summary>
        public ISubject<Unit> OnEndRightEquipmentSubject() => this.endRightEquipmentSubject;

        /// <summary>
        /// <inheritdoc cref="onHitOpponentAttack"/>
        /// </summary>
        public ISubject<EquipmentController> OnHitOpponentAttackSubject() => this.onHitOpponentAttack;

        /// <summary>
        /// <inheritdoc cref="onDead"/>
        /// </summary>
        public ISubject<Unit> OnDeadSubject() => this.onDead;

        /// <summary>
        /// <inheritdoc cref="onChangedStateType"/>
        /// </summary>
        public ISubject<ActorStateController.StateType> OnChangedStateSubject() => this.onChangedStateType;

        public void Dispose()
        {
            this.beginRightEquipmentSubject.Dispose();
            this.endRightEquipmentSubject.Dispose();
            this.onHitOpponentAttack.Dispose();
            this.onDead.Dispose();
            this.onChangedStateType.Dispose();
        }
    }
}
