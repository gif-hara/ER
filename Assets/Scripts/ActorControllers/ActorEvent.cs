using ER.EquipmentSystems;
using ER.MasterDataSystem;
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
    public sealed class ActorEvent
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
        /// 攻撃が当たった際のメッセージ
        /// </summary>
        public class OnHitAttack : Message<OnHitAttack, Vector3, Quaternion>
        {
            /// <summary>
            /// ヒットした座標
            /// </summary>
            public Vector3 HitPosition => this.param1;

            /// <summary>
            /// ヒットした角度
            /// </summary>
            public Quaternion Angle => this.param2;
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
        /// ロックオンを開始した際のメッセージ
        /// </summary>
        public class OnBeginLookAt : Message<OnBeginLookAt, Actor>
        {
            /// <summary>
            /// ロックオン対象
            /// </summary>
            public Actor Target => this.param1;
        }

        /// <summary>
        /// ロックオンを終了した際のメッセージ
        /// </summary>
        public class OnEndLookAt : Message<OnEndLookAt, Actor>
        {
            /// <summary>
            /// ロックオン対象
            /// </summary>
            public Actor Target => this.param1;
        }

        /// <summary>
        /// リスポーンされた際のメッセージ
        /// </summary>
        public class OnRespawned : Message<OnRespawned>
        {
        }

        /// <summary>
        /// <see cref="IInteractableStageGimmick"/>のエリア内に入った際のイベント
        /// </summary>
        public class OnEnterInteractableStageGimmick : Message<OnEnterInteractableStageGimmick, IInteractableStageGimmick>
        {
            /// <summary>
            /// ギミック
            /// </summary>
            public IInteractableStageGimmick Gimmick => this.param1;
        }

        /// <summary>
        /// <see cref="IInteractableStageGimmick"/>のエリアから出た際のイベント
        /// </summary>
        public class OnExitInteractableStageGimmick : Message<OnExitInteractableStageGimmick, IInteractableStageGimmick>
        {
            /// <summary>
            /// ギミック
            /// </summary>
            public IInteractableStageGimmick Gimmick => this.param1;
        }

        /// <summary>
        /// ダメージを受けた際のメッセージ
        /// </summary>
        public class OnTakedDamage : Message<OnTakedDamage, int>
        {
            /// <summary>
            /// ダメージ量
            /// </summary>
            public int Damage => this.param1;
        }

        /// <summary>
        /// 装備品の切り替えをリクエストするメッセージ
        /// </summary>
        public class OnRequestChangeEquipment : Message<OnRequestChangeEquipment, HandType>
        {
            /// <summary>
            /// 切り替えたい手のタイプ
            /// </summary>
            public HandType HandType => this.param1;
        }

        /// <summary>
        /// 手に持つ装備品が変更された際のメッセージ
        /// </summary>
        public class OnChangedHandEquipment : Message<OnChangedHandEquipment, HandType, EquipmentController>
        {
            /// <summary>
            /// 変更された手のタイプ
            /// </summary>
            public HandType HandType => this.param1;

            /// <summary>
            /// 装備品
            /// </summary>
            public EquipmentController EquipmentController => this.param2;
        }

        /// <summary>
        /// 回復アイテム使用開始をリクエストするメッセージ
        /// </summary>
        public class OnRequestStartRecoveryItem : Message<OnRequestStartRecoveryItem>
        {
        }

        /// <summary>
        /// チェックポイントとインタラクトした際のメッセージ
        /// </summary>
        public class OnInteractedCheckPoint : Message<OnInteractedCheckPoint, InteractableStageGimmickCheckPoint>
        {
            /// <summary>
            /// インタラクトしたチェックポイント
            /// </summary>
            public InteractableStageGimmickCheckPoint CheckPoint => this.param1;
        }

        /// <summary>
        /// アイテムを獲得した際のメッセージ
        /// </summary>
        public class OnAcquiredItem : Message<OnAcquiredItem, MasterDataItem.Record, int>
        {
            /// <summary>
            /// 獲得したアイテムのマスターデータ
            /// </summary>
            public MasterDataItem.Record MasterDataItem => this.param1;

            /// <summary>
            /// 獲得した数
            /// </summary>
            public int Number => this.param2;
        }
    }
}
