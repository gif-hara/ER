using System;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

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
        
        [SerializeField]
        private AnimatorOverrideController overrideController;

        private Animator animator;

        private const string OverrideClipName = "Clip";

        private void Start()
        {
            this.animator = this.GetComponent<Animator>();
            this.overrideController = new AnimatorOverrideController();
            this.overrideController.runtimeAnimatorController = this.animator.runtimeAnimatorController;
            this.animator.runtimeAnimatorController = this.overrideController;
            this.ChangeClip(this.idleClip);
            
            var actor = this.GetComponent<Actor>();
            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                .Subscribe(x =>
                {
                    if (x.NextState == ActorStateController.StateType.Movable)
                    {
                        this.ChangeClip(this.idleClip);
                    }
                })
                .AddTo(actor)
                .AddTo(this);
            // var animator = this.GetComponent<Animator>();
            //
            // actor.Broker.Receive<ActorEvent.OnRequestAvoidance>()
            //     .Subscribe(x =>
            //     {
            //         var direction = x.Direction;
            //         // 入力が無い場合はバックステップする
            //         if (direction == Vector2.zero)
            //         {
            //             direction = -actor.transform.up;
            //         }
            //
            //         direction = direction.normalized;
            //
            //         actor.DirectorController.PlayOneShotAsync(motionData.avoidanceAsset)
            //             .TakeUntil(this.actor.Broker.Receive<ActorEvent.BeginEquipment>())
            //             .TakeUntil(this.actor.Broker.Receive<ActorEvent.OnRequestAvoidance>())
            //             .Subscribe(_ =>
            //             {
            //                 actor.StateController.ChangeRequest(ActorStateController.StateType.Movable);
            //             })
            //             .AddTo(actor.Disposables);
            //
            //         actor.StateController.ChangeRequest(ActorStateController.StateType.Avoidance);
            //         actor.gameObject.UpdateAsObservable()
            //             .TakeUntil(actor.Broker.Receive<ActorEvent.OnChangedStateType>().Where(x => x.NextState != ActorStateController.StateType.Avoidance))
            //             .TakeUntil(this.actor.Broker.Receive<ActorEvent.OnRequestAvoidance>())
            //             .Subscribe(_ =>
            //             {
            //                 this.Move(direction);
            //             })
            //             .AddTo(actor.Disposables);
            //     })
            //     .AddTo(actor.Disposables);
        }

        private void ChangeClip(AnimationClip clip)
        {
            var layerInfos = new AnimatorStateInfo[this.animator.layerCount];
            for (var i = 0; i < this.animator.layerCount; i++)
            {
                layerInfos[i] = this.animator.GetCurrentAnimatorStateInfo(i);
            }
            
            Debug.Log(this.overrideController[OverrideClipName]);
            this.overrideController[OverrideClipName] = clip;
            this.animator.Update(0.0f);

            for (var i = 0; i < this.animator.layerCount; i++)
            {
                this.animator.Play(layerInfos[i].fullPathHash, i, layerInfos[i].normalizedTime);
            }
        }
    }
}
