using ER.EquipmentSystems;
using ER.StageControllers;
using HK.Framework.EventSystems;
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
        /// 装備品の使用を開始した際のメッセージ
        /// </summary>
        public class BeginEquipment : Message<BeginEquipment, HandType>
        {
            /// <summary>
            /// どの手の装備品を使用したか
            /// </summary>
            public HandType HandType => this.param1;
        }

        /// <summary>
        /// 装備品の使用を終了した際のメッセージ
        /// </summary>
        public class EndEquipment : Message<EndEquipment, HandType>
        {
            /// <summary>
            /// どの手の装備品の使用を終了したか
            /// </summary>
            public HandType HandType => this.param1;
        }

        /// <summary>
        /// 相手から攻撃を受けた際のメッセージ
        /// </summary>
        public class OnHitOpponentAttack : Message<OnHitOpponentAttack, EquipmentController>
        {
            /// <summary>
            /// 相手側の<see cref="EquipmentController"/>
            /// </summary>
            public EquipmentController OpponentEquipmentController => this.param1;
        }

        /// <summary>
        /// 死亡した際のメッセージ
        /// </summary>
        public class OnDead : Message<OnDead>
        {
        }

        /// <summary>
        /// ステートが切り替わった際のメッセージ
        /// </summary>
        public class OnChangedStateType : Message<OnChangedStateType, ActorStateController.StateType>
        {
            /// <summary>
            /// 次のステート
            /// </summary>
            public ActorStateController.StateType NextState => this.param1;
        }

        /// <summary>
        /// 回避をリクエストするメッセージ
        /// </summary>
        public class OnRequestAvoidance : Message<OnRequestAvoidance, Vector2>
        {
            /// <summary>
            /// 回避する方向
            /// </summary>
            public Vector2 Direction => this.param1;
        }

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
        /// ダメージを受けた際のイベント
        /// </summary>
        private readonly Subject<int> onTakedDamage = new Subject<int>();

        /// <summary>
        /// 右手装備品の切り替えをリクエストするイベント
        /// </summary>
        private readonly Subject<Unit> onRequestChangeRightEquipment = new Subject<Unit>();

        /// <summary>
        /// 左手装備品の切り替えをリクエストするイベント
        /// </summary>
        private readonly Subject<Unit> onRequestChangeLeftEquipment = new Subject<Unit>();

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

        /// <summary>
        /// <inheritdoc cref="onTakedDamage"/>
        /// </summary>
        public ISubject<int> OnTakedDamageSubject() => this.onTakedDamage;

        /// <summary>
        /// <inheritdoc cref="onRequestChangeRightEquipment"/>
        /// </summary>
        public ISubject<Unit> OnRequestChangeRightEquipment() => this.onRequestChangeRightEquipment;

        /// <summary>
        /// <inheritdoc cref="onRequestChangeLeftEquipment"/>
        /// </summary>
        public ISubject<Unit> OnRequestChangeLeftEquipment() => this.onRequestChangeLeftEquipment;

        public void Dispose()
        {
            this.onBeginLookAt.Dispose();
            this.onEndLookAt.Dispose();
            this.onRespawned.Dispose();
            this.onEnterInteractableStageGimmick.Dispose();
            this.onExitInteractableStageGimmick.Dispose();
            this.onTakedDamage.Dispose();
            this.onRequestChangeRightEquipment.Dispose();
            this.onRequestChangeLeftEquipment.Dispose();
        }
    }
}
