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

        /// <summary>
        /// ノックバックによる移動量
        /// </summary>
        private Vector2 knockBackVelocity;

        /// <summary>
        /// 向いている方向
        /// </summary>
        private float angle;

        /// <summary>
        /// 注視している<see cref="Actor"/>
        /// </summary>
        private Actor lookAtTarget;

        /// <summary>
        /// チェックポイント
        /// </summary>
        public Vector3 CheckPoint { get; set; }

        /// <summary>
        /// 注視しているか返す
        /// </summary>
        public bool IsLookAt { get; private set; }

        /// <summary>
        /// 利用可能な状態にする
        /// </summary>
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
            
            actor.Broker.Receive<ActorEvent.OnRespawned>()
                .Subscribe(_ =>
                {
                    if (IsLookAt)
                    {
                        this.EndLookAt();
                    }
                    
                    this.knockBackVelocity = Vector2.zero;
                })
                .AddTo(actor.Disposables);

            actor.Broker.Receive<ActorEvent.OnInteractedCheckPoint>()
                .Subscribe(x =>
                {
                    this.CheckPoint = x.CheckPoint.transform.position;
                })
                .AddTo(actor.Disposables);

            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                .Subscribe(x =>
                {
                    if (x.NextState == ActorStateController.StateType.KnockBack)
                    {
                        // ノックバック値が無くなったら移動可能になる
                        this.actor.gameObject.UpdateAsObservable()
                            .TakeUntil(this.actor.Broker.Receive<ActorEvent.OnChangedStateType>())
                            .Where(_ => this.knockBackVelocity.sqrMagnitude <= 0)
                            .Subscribe(_ =>
                            {
                                this.actor.StateController.ChangeRequest(ActorStateController.StateType.Movable);
                            });
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

        /// <summary>
        /// ノックバック移動量を加算する
        /// </summary>
        public void AddKnockBack(Vector2 velocity)
        {
            this.knockBackVelocity += velocity;
        }

        /// <summary>
        /// 回転を行う
        /// </summary>
        public void Rotate(float angle)
        {
            this.angle = angle;
        }

        /// <summary>
        /// 回転を行う
        /// </summary>
        public void Rotate(Vector2 angle)
        {
            this.Rotate(-90.0f + Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg);
        }

        /// <summary>
        /// 注視を開始する
        /// </summary>
        public void BeginLookAt(Actor target)
        {
            this.lookAtTarget = target;
            Assert.IsNotNull(this.lookAtTarget);
            this.IsLookAt = true;

            this.actor.Broker.Publish(ActorEvent.OnBeginLookAt.Get(this.lookAtTarget));
        }

        /// <summary>
        /// 注視を終了する
        /// </summary>
        public void EndLookAt()
        {
            Assert.IsTrue(this.IsLookAt);
            var tempTarget = this.lookAtTarget;
            this.lookAtTarget = null;
            this.IsLookAt = false;

            this.actor.Broker.Publish(ActorEvent.OnEndLookAt.Get(tempTarget));
        }

        /// <summary>
        /// 座標の更新
        /// </summary>
        private void UpdatePosition(IActor actor)
        {
            var velocity =
                (this.moveDirection * Time.deltaTime * this.motionData.moveSpeed * actor.AnimationParameter.moveSpeedRate)
                + this.rawVelocity
                + this.knockBackVelocity * Time.deltaTime;
            var t = this.actor.transform;
            this.actor.Rigidbody2D.MovePosition(new Vector2(t.position.x, t.position.y) + velocity);
            this.moveDirection = Vector2.zero;
            this.rawVelocity = Vector2.zero;

            if (this.knockBackVelocity.magnitude >= 0.1f)
            {
                this.knockBackVelocity -= this.knockBackVelocity.normalized * Time.deltaTime * 30.0f;
            }
            else
            {
                this.knockBackVelocity = Vector2.zero;
            }
        }

        /// <summary>
        /// 回転の更新
        /// </summary>
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
