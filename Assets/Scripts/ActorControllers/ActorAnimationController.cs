using System;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using UniRx.Triggers;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorAnimationController : MonoBehaviour
    {
        [SerializeField]
        private AnimationClip idleClip = default;

        [SerializeField]
        private AnimationClip avoidanceClip = default;

        private const string OverrideClipName = "Clip";

        public AnimatorOverrideController OverrideController { get; private set; }

        public Animator Animator { get; private set; }

        private void Start()
        {
            this.Animator = this.GetComponent<Animator>();
            this.OverrideController = new AnimatorOverrideController();
            this.OverrideController.runtimeAnimatorController = this.Animator.runtimeAnimatorController;
            this.Animator.runtimeAnimatorController = this.OverrideController;
            this.ChangeClip(this.idleClip);

            var actor = this.GetComponent<Actor>();
            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                 .Subscribe(x =>
                            {
                                if (x.NextState == ActorStateController.StateType.Movable)
                                {
                                    this.ChangeClip(this.idleClip);
                                }

                                if (x.NextState == ActorStateController.StateType.KnockBack)
                                {
                                    this.ChangeClip(this.idleClip);
                                }
                            })
                 .AddTo(actor)
                 .AddTo(this);

            actor.Broker.Receive<ActorEvent.OnRequestAvoidance>()
                 .Subscribe(x =>
                            {
                                var direction = x.Direction;

                                // 入力が無い場合はバックステップする
                                if (direction == Vector2.zero)
                                {
                                    direction = -actor.transform.up;
                                }

                                direction = direction.normalized;

                                this.PlayOneShotAsync(this.avoidanceClip)
                                    .TakeUntil(actor.Broker.Receive<ActorEvent.BeginEquipment>())
                                    .TakeUntil(actor.Broker.Receive<ActorEvent.OnRequestAvoidance>())
                                    .Subscribe(_ =>
                                               {
                                                   actor.StateController.ChangeRequest(
                                                       ActorStateController.StateType.Movable);
                                               })
                                    .AddTo(actor.Disposables);

                                actor.StateController.ChangeRequest(ActorStateController.StateType.Avoidance);
                                actor.gameObject.UpdateAsObservable()
                                     .TakeUntil(actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                                                     .Where(x => x.NextState != ActorStateController.StateType.Avoidance))
                                     .TakeUntil(actor.Broker.Receive<ActorEvent.OnRequestAvoidance>())
                                     .Subscribe(_ => { actor.MotionController.Move(direction); })
                                     .AddTo(actor.Disposables);
                            })
                 .AddTo(actor.Disposables);
        }

        public IObservable<Unit> PlayOneShotAsync(AnimationClip clip)
        {
            this.ChangeClip(clip);

            return this.UpdateAsObservable()
                       .Where(_ => this.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                       .Take(1)
                       .AsUnitObservable();
        }

        public void Play(AnimationClip clip)
        {
            this.ChangeClip(clip);
        }

        private void ChangeClip(AnimationClip clip)
        {
            this.OverrideController[OverrideClipName] = clip;

            for (var i = 0;
                 i < this.Animator.layerCount;
                 i++)
            {
                this.Animator.Play(this.Animator.GetCurrentAnimatorStateInfo(i).fullPathHash,
                                   i,
                                   0.0f);
            }

            this.Animator.Update(0.0f);
        }
    }
}