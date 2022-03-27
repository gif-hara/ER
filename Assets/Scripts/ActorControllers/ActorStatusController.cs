using ER.EquipmentSystems;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorStatusController
    {
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

        private ReactiveProperty<int> recoveryItemNumber = new ReactiveProperty<int>(RecoveryItemMax);

        public IObservable<int> RecoveryItemNumberAsObservable() => this.recoveryItemNumber;

        public int RecoveryItemNumber => this.recoveryItemNumber.Value;

        public ActorStatusData BaseStatus => this.baseStatus;

        public void Setup(IActor actor, ActorStatusData status)
        {
            this.actor = actor;
            this.baseStatus = status;
            this.hitPointMax.Value = this.baseStatus.hitPoint;
            this.hitPoint.Value = this.HitPointMax;
            this.experience.Value = this.baseStatus.experience;

            actor.Broker.Receive<ActorEvent.OnHitOpponentAttack>()
                .Where(_ => this.CanTakeDamage())
                .Subscribe(x =>
                {
                    this.TakeDamage(x.OpponentEquipmentController);
                })
                .AddTo(actor.Disposables);

            actor.Broker.Receive<ActorEvent.OnRespawned>()
                .Subscribe(_ =>
                {
                    this.hitPoint.Value = this.HitPointMax;
                    this.isAlreadyDead = false;
                })
                .AddTo(actor.Disposables);

            actor.Broker.Receive<ActorEvent.OnInteractedCheckPoint>()
                .Subscribe(_ =>
                {
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
