using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorMotionController
    {
        private ActorMotionData motionData;

        /// <summary>
        /// 移動する方向
        /// </summary>
        private Vector2 moveDirection;

        private float angle;

        private RaycastHit2D[] cachedRaycastHit2Ds = new RaycastHit2D[32];

        public void Setup(IActor actor, ActorMotionData motionData, CompositeDisposable disposable)
        {
            this.motionData = motionData;
            actor.gameObject.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    UpdatePosition(actor);
                    UpdateRotation(actor);
                })
                .AddTo(disposable);

            actor.Event.OnChangedStateSubject()
                .Subscribe(x =>
                {
                    if (x == ActorStateController.StateType.Movable)
                    {
                        actor.AnimationParameter.moveSpeedRate = 1.0f;
                    }
                })
                .AddTo(disposable);
        }

        public void Move(Vector2 moveDirection)
        {
            this.moveDirection = moveDirection;
        }

        public void Rotate(float angle)
        {
            this.angle = angle;
        }

        public void Rotate(Vector2 angle)
        {
            this.Rotate(-90.0f + Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg);
        }

        private void UpdatePosition(IActor actor)
        {
            var t = actor.transform;
            var direction = this.moveDirection.normalized;
            var velocity = this.moveDirection * Time.deltaTime * this.motionData.moveSpeed * actor.AnimationParameter.moveSpeedRate;
            var hitNumber = Physics2D.CircleCastNonAlloc(
                t.localPosition,
                this.motionData.radius,
                direction,
                this.cachedRaycastHit2Ds,
                velocity.magnitude,
                Layer.Mask.Stage
                );

            if (hitNumber > 0)
            {
                var hitinfo = this.cachedRaycastHit2Ds[0];
                t.localPosition = hitinfo.point + hitinfo.normal * this.motionData.radius;

            }
            else
            {
                t.localPosition += new Vector3(velocity.x, velocity.y, 0.0f);
            }

            this.moveDirection = Vector2.zero;
        }

        private void UpdateRotation(IActor actor)
        {
            actor.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
    }
}
