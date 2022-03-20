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

        public ActorStatusData BaseStatus => this.baseStatus;

        public void Setup(IActor actor, ActorStatusData status)
        {
            this.actor = actor;
            this.baseStatus = status;
            this.hitPointMax.Value = this.baseStatus.hitPoint;
            this.hitPoint.Value = this.HitPointMax;

            actor.Event.OnHitOpponentAttackSubject()
                .Where(_ => this.CanTakeDamage())
                .Subscribe(x =>
                {
                    this.TakeDamage(x);
                })
                .AddTo(actor.Disposables);

            actor.Event.OnRespawnedSubject()
                .Subscribe(_ =>
                {
                    this.hitPoint.Value = this.HitPointMax;
                    this.isAlreadyDead = false;
                })
                .AddTo(actor.Disposables);
        }

        private void TakeDamage(EquipmentController equipmentController)
        {
            if (this.isAlreadyDead)
            {
                return;
            }

            var attackerStatus = equipmentController.Actor.StatusController.baseStatus;
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
            this.actor.Event.OnTakedDamageSubject().OnNext(damage);

            if (this.HitPoint <= 0)
            {
                this.isAlreadyDead = true;
                this.actor.Event.OnDeadSubject().OnNext(Unit.Default);
            }
        }

        private bool CanTakeDamage()
        {
            return !this.actor.AnimationParameter.invisible;
        }
    }
}
