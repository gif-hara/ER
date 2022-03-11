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

        public int HitPointMax { get; private set; }

        public int HitPoint { get; private set; }

        public void Setup(IActor actor, ActorStatusData status, CompositeDisposable disposable)
        {
            this.actor = actor;
            this.baseStatus = status;
            this.HitPointMax = this.baseStatus.HitPoint;
            this.HitPoint = this.HitPointMax;

            actor.Event.OnHitOpponentAttackSubject()
                .Subscribe(x =>
                {
                    this.TakeDamage(x.Power);
                })
                .AddTo(disposable);
        }

        private void TakeDamage(int damage)
        {
            if(this.isAlreadyDead)
            {
                return;
            }

            this.HitPoint -= damage;
            if(this.HitPoint <= 0)
            {
                this.isAlreadyDead = true;
                this.actor.Event.OnDeadSubject().OnNext(Unit.Default);
            }
        }
    }
}
