using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 移動や回転を制御するクラス
    /// </summary>
    public sealed class ActorMotionController
    {
        private IActor actor;

        private ActorMotionData motionData;

        /// <summary>
        /// 移動する方向
        /// </summary>
        private Vector2 moveDirection;

        /// <summary>
        /// 直接加算される移動量
        /// </summary>
        private Vector2 rawVelocity;

        private float angle;

        private Actor lookAtTarget;

        /// <summary>
        /// チェックポイント
        /// </summary>
        public Vector3 CheckPoint { get; set; }

        /// <summary>
        /// 注視しているか返す
        /// </summary>
        public bool IsLookAt { get; private set; }

        public void Setup(IActor actor, ActorMotionData motionData)
        {
            this.actor = actor;
            this.motionData = motionData;
            this.CheckPoint = this.actor.transform.position;
            actor.gameObject.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    UpdatePosition(actor);
                    UpdateRotation(actor);
                })
                .AddTo(actor.Disposables);

            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                .Subscribe(x =>
                {
                    if (x.NextState == ActorStateController.StateType.Movable)
                    {
                        actor.AnimationParameter.moveSpeedRate = 1.0f;
                        actor.AnimationParameter.invisible = false;
                        actor.AnimationParameter.advancedEntry = true;
                    }

                    if (x.NextState == ActorStateController.StateType.Guard)
                    {
                        actor.AnimationParameter.moveSpeedRate = 0.5f;
                        actor.AnimationParameter.invisible = false;
                        actor.AnimationParameter.advancedEntry = true;
                    }

                })
                .AddTo(actor.Disposables);

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

                    actor.DirectorController.PlayOneShotAsync(motionData.avoidanceAsset)
                    .TakeUntil(this.actor.Broker.Receive<ActorEvent.BeginEquipment>())
                    .TakeUntil(this.actor.Broker.Receive<ActorEvent.OnRequestAvoidance>())
                    .Subscribe(_ =>
                    {
                        actor.StateController.ChangeRequest(ActorStateController.StateType.Movable);
                    })
                    .AddTo(actor.Disposables);

                    actor.StateController.ChangeRequest(ActorStateController.StateType.Avoidance);
                    actor.gameObject.UpdateAsObservable()
                    .TakeUntil(actor.Broker.Receive<ActorEvent.OnChangedStateType>().Where(x => x.NextState != ActorStateController.StateType.Avoidance))
                    .TakeUntil(this.actor.Broker.Receive<ActorEvent.OnRequestAvoidance>())
                    .Subscribe(_ =>
                    {
                        this.Move(direction);
                    })
                    .AddTo(actor.Disposables);
                })
                .AddTo(actor.Disposables);

            actor.Broker.Receive<ActorEvent.OnRespawned>()
                .Subscribe(_ =>
                {
                    if (IsLookAt)
                    {
                        this.EndLookAt();
                    }
                })
                .AddTo(actor.Disposables);
        }

        /// <summary>
        /// 移動量を設定する
        /// </summary>
        /// <remarks>
        /// 実際の移動時に<see cref="Time.deltaTime"/>が考慮されます
        /// </remarks>
        public void Move(Vector2 moveDirection)
        {
            this.moveDirection += moveDirection;
        }

        /// <summary>
        /// 移動量を設定する
        /// </summary>
        /// <remarks>
        /// 実際の移動時に<see cref="Time.deltaTime"/>が考慮されません
        /// </remarks>
        public void MoveRaw(Vector2 rawVelocity)
        {
            this.rawVelocity += rawVelocity;
        }

        public void Rotate(float angle)
        {
            this.angle = angle;
        }

        public void Rotate(Vector2 angle)
        {
            this.Rotate(-90.0f + Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg);
        }

        public void BeginLookAt(Actor target)
        {
            this.lookAtTarget = target;
            Assert.IsNotNull(this.lookAtTarget);
            this.IsLookAt = true;

            this.actor.Broker.Publish(ActorEvent.OnBeginLookAt.Get(this.lookAtTarget));
        }

        public void EndLookAt()
        {
            Assert.IsTrue(this.IsLookAt);
            var tempTarget = this.lookAtTarget;
            this.lookAtTarget = null;
            this.IsLookAt = false;

            this.actor.Broker.Publish(ActorEvent.OnEndLookAt.Get(tempTarget));
        }

        private void UpdatePosition(IActor actor)
        {
            var velocity =
                (this.moveDirection * Time.deltaTime * this.motionData.moveSpeed * actor.AnimationParameter.moveSpeedRate)
                + this.rawVelocity;
            var t = this.actor.transform;
            this.actor.Rigidbody2D.MovePosition(new Vector2(t.position.x, t.position.y) + velocity);
            this.moveDirection = Vector2.zero;
            this.rawVelocity = Vector2.zero;
        }

        private void UpdateRotation(IActor actor)
        {
            if (this.IsLookAt)
            {
                if (this.lookAtTarget == null)
                {
                    this.EndLookAt();
                }
                else
                {
                    var direction = (this.lookAtTarget.transform.position - actor.transform.position).normalized;
                    angle = -90.0f + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                }
            }

            actor.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
    }
}
