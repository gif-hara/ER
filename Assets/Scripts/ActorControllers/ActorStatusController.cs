using ER.EquipmentSystems;
using System;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// アクターのステータスを制御するクラス
    /// </summary>
    public sealed class ActorStatusController
    {
        /// <summary>
        /// 回復アイテム所持数
        /// TODO: 動的に増やせるようにしたほうがいいかも
        /// </summary>
        public const int RecoveryItemMax = 3;

        private IActor actor;

        private ActorStatusData baseStatus = default;

        private bool isAlreadyDead = false;

        private readonly ReactiveProperty<int> hitPointMax = new ReactiveProperty<int>();

        private readonly ReactiveProperty<int> hitPoint = new ReactiveProperty<int>();

        public IObservable<int> HitPointMaxAsObservable() => this.hitPointMax;

        public IObservable<int> HitPointAsObservable() => this.hitPoint;

        public int HitPointMax => this.hitPointMax.Value;

        public int HitPoint => this.hitPoint.Value;

        public float HitPointRate => (float)this.HitPoint / this.HitPointMax;

        private readonly ReactiveProperty<int> experience = new ReactiveProperty<int>();

        public IObservable<int> ExperienceAsObservable() => this.experience;

        public int Experience => this.experience.Value;

        private readonly ReactiveProperty<int> recoveryItemNumber = new ReactiveProperty<int>(RecoveryItemMax);

        public IObservable<int> RecoveryItemNumberAsObservable() => this.recoveryItemNumber;

        public int RecoveryItemNumber => this.recoveryItemNumber.Value;

        public ActorStatusData BaseStatus => this.baseStatus;

        public void Setup(IActor actor, ActorStatusData status)
        {
            this.actor = actor;
            this.baseStatus = status;
            
            // 基本ステータスから初期化を行う
            this.hitPointMax.Value = this.baseStatus.hitPoint;
            this.hitPoint.Value = this.HitPointMax;
            this.experience.Value = this.baseStatus.experience;

            // 相手の攻撃が当たったらダメージを受ける
            actor.Broker.Receive<ActorEvent.OnHitOpponentAttack>()
                .Where(_ => this.CanTakeDamage())
                .Subscribe(x =>
                {
                    this.TakeDamage(x.OpponentEquipmentController);

                    var knockbackVelocity = x.OpponentEquipmentController.transform.up * x.OpponentEquipmentController.KnockbackPower;
                    this.actor.MotionController.AddKnockback(knockbackVelocity);
                })
                .AddTo(actor.Disposables);

            // リスポーン時に各種パラメータを設定する
            actor.Broker.Receive<ActorEvent.OnRespawned>()
                .Subscribe(_ =>
                {
                    this.hitPoint.Value = this.HitPointMax;
                    this.isAlreadyDead = false;
                    this.recoveryItemNumber.Value = RecoveryItemMax;
                })
                .AddTo(actor.Disposables);
            
            // チェックポイントに入ったら各種パラメータを設定する
            actor.Broker.Receive<ActorEvent.OnInteractedCheckPoint>()
                .Subscribe(_ =>
                {
                    this.hitPoint.Value = this.HitPointMax;
                    this.recoveryItemNumber.Value = RecoveryItemMax;
                })
                .AddTo(actor.Disposables);
        }

        public void AddExperience(int value)
        {
            this.experience.Value += value;
        }

        public void UseRecoveryItem()
        {
            Assert.IsTrue(this.CanUseRecoveryItem());

            this.recoveryItemNumber.Value -= 1;

            // TODO: 回復量決める
            var hitPoint = this.HitPoint;
            hitPoint = Mathf.Min(hitPoint + this.HitPointMax / 2, this.HitPointMax);

            this.hitPoint.Value = hitPoint;
        }

        public bool CanUseRecoveryItem()
        {
            return this.RecoveryItemNumber > 0;
        }

        private void TakeDamage(EquipmentController equipmentController)
        {
            if (this.isAlreadyDead)
            {
                return;
            }

            var damage = DamageCalculator.Calculate(
                equipmentController.Actor,
                equipmentController,
                this.actor,
                equipmentController.Power
                );
            TakeDamage(damage);
        }

        private void TakeDamage(int damage)
        {
            if (this.isAlreadyDead)
            {
                return;
            }

            this.hitPoint.Value -= damage;
            this.actor.Broker.Publish(ActorEvent.OnTakedDamage.Get(damage));

            if (this.HitPoint <= 0)
            {
                this.isAlreadyDead = true;
                this.actor.Broker.Publish(ActorEvent.OnDead.Get());
            }
        }

        private bool CanTakeDamage()
        {
            return !this.actor.AnimationParameter.invisible;
        }
    }
}
