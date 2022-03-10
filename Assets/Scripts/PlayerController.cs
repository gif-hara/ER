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
        private float moveSpeed = default;

        [SerializeField]
        private float angleThreshold = default;

        [SerializeField]
        private float radius = default;

        [SerializeField]
        private EquipmentController rightEquipmentPrefab = default;

        private ERInputAction inputAction;

        private RaycastHit2D[] cachedRaycastHit2Ds = new RaycastHit2D[32];

        private void Start()
        {
            this.inputAction = new ERInputAction();
            this.inputAction.Enable();

            this.inputAction.Player.Fire.performed += OnPerformedBeginRightEquipment;
            this.inputAction.Player.Fire.canceled += OnCanceledBeginRightEquipment;

            this.actor.SetRightEquipment(this.rightEquipmentPrefab);
        }

        private void Update()
        {
            var t = this.transform;
            var angle = this.inputAction.Player.Look.ReadValue<Vector2>();
            if (angle.sqrMagnitude > this.angleThreshold * this.angleThreshold)
            {
                t.localRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f + Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg);
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
