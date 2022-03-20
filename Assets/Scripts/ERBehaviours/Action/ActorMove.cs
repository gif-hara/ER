using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorMove : IAction
    {
        [SerializeField]
        private float offsetAngle;

        [SerializeField]
        private float moveSpeed = 1.0f;

        public void Invoke(IBehaviourData data)
        {
            var actorHolder = data.Cast<IActorHolder>();
            var actor = actorHolder.Actor;
            var angle = 90 + actor.transform.rotation.eulerAngles.z + this.offsetAngle;
            var direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
                );
            actor.MotionController.Move(direction * this.moveSpeed);
        }
    }
}
