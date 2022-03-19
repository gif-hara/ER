using ER.EquipmentSystems;
using ER.StageControllers;
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
        /// 左手装備品の使用を開始した際のイベント
        /// </summary>
        private readonly Subject<Unit> beginLeftEquipmentSubject = new Subject<Unit>();
        
        /// <summary>
        /// 左手装備品の使用を完了した際のイベント
        /// </summary>
        private readonly Subject<Unit> endLeftEquipmentSubject = new Subject<Unit>();

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
        /// 回避をリクエストするイベント
        /// </summary>
        private readonly Subject<Vector2> onRequestAvoidance = new Subject<Vector2>();

        /// <summary>
        /// ロックオンを開始した際のイベント
        /// </summary>
        private readonly Subject<Actor> onBeginLookAt = new Subject<Actor>();

        /// <summary>
        /// ロックオンを終了した際のイベント
        /// </summary>
        private readonly Subject<Actor> onEndLookAt = new Subject<Actor>();

        /// <summary>
        /// リスポーンされた際のイベント
        /// </summary>
        private readonly Subject<Unit> onRespawned = new Subject<Unit>();

        /// <summary>
        /// <see cref="IInteractableGimmick"/>のエリア内に入った際のイベント
        /// </summary>
        private readonly Subject<IInteractableStageGimmick> onEnterInteractableStageGimmick = new Subject<IInteractableStageGimmick>();

        /// <summary>
        /// <see cref="IInteractableGimmick"/>のエリアから出た際のイベント
        /// </summary>
        private readonly Subject<IInteractableStageGimmick> onExitInteractableStageGimmick = new Subject<IInteractableStageGimmick>();

        /// <summary>
        /// <inheritdoc cref="beginRightEquipmentSubject"/>
        /// </summary>
        public ISubject<Unit> OnBeginRightEquipmentSubject() => this.beginRightEquipmentSubject;

        /// <summary>
        /// <inheritdoc cref="endRightEquipmentSubject"/>
        /// </summary>
        public ISubject<Unit> OnEndRightEquipmentSubject() => this.endRightEquipmentSubject;

        /// <summary>
        /// <inheritdoc cref="beginLeftEquipmentSubject"/>
        /// </summary>
        public ISubject<Unit> OnBeginLeftEquipmentSubject() => this.beginLeftEquipmentSubject;

        /// <summary>
        /// <inheritdoc cref="endLeftEquipmentSubject"/>
        /// </summary>
        public ISubject<Unit> OnEndLeftEquipmentSubject() => this.endLeftEquipmentSubject;

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

        /// <summary>
        /// <inheritdoc cref="onRequestAvoidance"/>
        /// </summary>
        public ISubject<Vector2> OnRequestAvoidanceSubject() => this.onRequestAvoidance;

        /// <summary>
        /// <inheritdoc cref="onBeginLookAt"/>
        /// </summary>
        public ISubject<Actor> OnBeginLookAtSubject() => this.onBeginLookAt;

        /// <summary>
        /// <inheritdoc cref="onEndLookAt"/>
        /// </summary>
        public ISubject<Actor> OnEndLookAtSubject() => this.onEndLookAt;

        /// <summary>
        /// <inheritdoc cref="onRespawned"/>
        /// </summary>
        public ISubject<Unit> OnRespawnedSubject() => this.onRespawned;

        /// <summary>
        /// <inheritdoc cref="onEnterInteractableStageGimmick"/>
        /// </summary>
        public ISubject<IInteractableStageGimmick> OnEnterInteractableStageGimmickSubject() => this.onEnterInteractableStageGimmick;

        /// <summary>
        /// <inheritdoc cref="onExitInteractableStageGimmick"/>
        /// </summary>
        public ISubject<IInteractableStageGimmick> OnExitInteractableStageGimmickSubject() => this.onExitInteractableStageGimmick;

        public void Dispose()
        {
            this.beginRightEquipmentSubject.Dispose();
            this.endRightEquipmentSubject.Dispose();
            this.onHitOpponentAttack.Dispose();
            this.onDead.Dispose();
            this.onChangedStateType.Dispose();
            this.onRequestAvoidance.Dispose();
            this.onBeginLookAt.Dispose();
            this.onEndLookAt.Dispose();
            this.onRespawned.Dispose();
            this.onEnterInteractableStageGimmick.Dispose();
            this.onExitInteractableStageGimmick.Dispose();
        }
    }
}
