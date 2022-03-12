using ER.ActorControllers;
using ER.EquipmentSystems;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;

        [SerializeField]
        private float angleThreshold = default;

        [SerializeField]
        private EquipmentController rightEquipmentPrefab = default;

        private ERInputAction inputAction;

        private void Start()
        {
            this.inputAction = new ERInputAction();
            this.inputAction.Enable();

            this.inputAction.Player.Fire.performed += OnPerformedBeginRightEquipment;
            this.inputAction.Player.Fire.canceled += OnCanceledBeginRightEquipment;
            this.inputAction.Player.Avoidance.performed += callback =>
            {
                var direction = this.inputAction.Player.Move.ReadValue<Vector2>();
                this.actor.Event.OnRequestAvoidanceSubject().OnNext(direction);
            };
            this.inputAction.Player.LookAt.performed += callback =>
            {
                if(!this.actor.MotionController.IsLookAt)
                {
                    this.actor.MotionController.BeginLookAt(Actor.Enemies[0].transform);
                }
                else
                {
                    this.actor.MotionController.EndLookAt();
                }
            };

            this.actor.SetRightEquipment(this.rightEquipmentPrefab);
        }

        private void Update()
        {
            if(this.actor.StateController.CurrentState != ActorStateController.StateType.Movable)
            {
                return;
            }
            var t = this.transform;
            var angle = this.inputAction.Player.Look.ReadValue<Vector2>();
            if (angle.sqrMagnitude > this.angleThreshold * this.angleThreshold)
            {
                this.actor.MotionController.Rotate(angle);
            }

            var direction = this.inputAction.Player.Move.ReadValue<Vector2>();
            this.actor.MotionController.Move(direction);
        }

        private void OnPerformedBeginRightEquipment(InputAction.CallbackContext callback)
        {
            this.actor.Event.OnBeginRightEquipmentSubject().OnNext(Unit.Default);
        }

        private void OnCanceledBeginRightEquipment(InputAction.CallbackContext callback)
        {
            this.actor.Event.OnEndRightEquipmentSubject().OnNext(Unit.Default);
        }
    }
}
