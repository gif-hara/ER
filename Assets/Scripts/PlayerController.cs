using ER.ActorControllers;
using ER.EquipmentSystems;
using ER.MasterDataSystem;
using I2.Loc;
using System;
using System.Collections.Generic;
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

        [SerializeReference, SubclassSelector(typeof(IEquipmentSelector))]
        private IEquipmentSelector rightEquipmentSelector = default;

        [SerializeReference, SubclassSelector(typeof(IEquipmentSelector))]
        private IEquipmentSelector leftEquipmentSelector = default;

        [SerializeField, TermsPopup("ArmorHead/")]
        private string headMasterDataId = default;

        [SerializeField, TermsPopup("ArmorTorso/")]
        private string torsoMasterDataId = default;

        [SerializeField, TermsPopup("ArmorArm/")]
        private string armMasterDataId = default;

        [SerializeField, TermsPopup("ArmorLeg/")]
        private string legMasterDataId = default;

        /// <summary>
        /// ロックオン可能な距離の閾値
        /// </summary>
        [SerializeField]
        private float lookAtDistanceThreshold = default;

        /// <summary>
        /// ロックオン可能な角度の閾値
        /// </summary>
        [SerializeField]
        private float lookAtAngleThreshold = default;

        private ERInputAction inputAction;

        private void Start()
        {
            this.inputAction = new ERInputAction();
            this.inputAction.Enable();

            this.inputAction.Player.UseRightEquipment.performed += callback =>
            {
                if (!this.CanBeginEquipmentSubject())
                {
                    return;
                }

                this.actor.Event.OnBeginRightEquipmentSubject().OnNext(Unit.Default);
            };
            this.inputAction.Player.UseRightEquipment.canceled += callback =>
            {
                this.actor.Event.OnEndRightEquipmentSubject().OnNext(Unit.Default);
            };
            this.inputAction.Player.UseLeftEquipment.performed += callback =>
            {
                if (!this.CanBeginEquipmentSubject())
                {
                    return;
                }

                this.actor.Event.OnBeginLeftEquipmentSubject().OnNext(Unit.Default);
            };
            this.inputAction.Player.UseLeftEquipment.canceled += callback =>
            {
                this.actor.Event.OnEndLeftEquipmentSubject().OnNext(Unit.Default);
            };
            this.inputAction.Player.Avoidance.performed += callback =>
            {
                if (!this.CanAvoidance())
                {
                    return;
                }

                var direction = this.inputAction.Player.Move.ReadValue<Vector2>();
                this.actor.Event.OnRequestAvoidanceSubject().OnNext(direction);
            };
            this.inputAction.Player.LookAt.performed += callback =>
            {
                if (!this.actor.MotionController.IsLookAt)
                {
                    var target = FindLookAtTargetEnemy();
                    if (target == null)
                    {
                        return;
                    }
                    this.actor.MotionController.BeginLookAt(target);
                }
                else
                {
                    this.actor.MotionController.EndLookAt();
                }
            };

            this.inputAction.Player.Interact.performed += callback =>
            {
                this.actor.InteractableStageGimmickController.BeginInteract();
            };

            this.rightEquipmentSelector.Attach(this.actor);
            this.leftEquipmentSelector.Attach(this.actor);

            this.actor.EquipmentController.SetArmor(ArmorType.Head, this.headMasterDataId);
            this.actor.EquipmentController.SetArmor(ArmorType.Torso, this.torsoMasterDataId);
            this.actor.EquipmentController.SetArmor(ArmorType.Arm, this.armMasterDataId);
            this.actor.EquipmentController.SetArmor(ArmorType.Leg, this.legMasterDataId);
        }

        private void Update()
        {
            var t = this.transform;
            var angle = this.inputAction.Player.Look.ReadValue<Vector2>();
            if (angle.sqrMagnitude > this.angleThreshold * this.angleThreshold)
            {
                this.actor.MotionController.Rotate(angle);
            }

            var direction = this.inputAction.Player.Move.ReadValue<Vector2>();
            this.actor.MotionController.Move(direction);
        }

        private void OnDrawGizmosSelected()
        {
            var currentAngle = this.transform.rotation.eulerAngles.z;
            var from = this.transform.position;
            var min = (90 + currentAngle - this.lookAtAngleThreshold) * Mathf.Deg2Rad;
            var max = (90 + currentAngle + this.lookAtAngleThreshold) * Mathf.Deg2Rad;

            var tempColor = Gizmos.color;
            Gizmos.DrawWireSphere(from, this.lookAtDistanceThreshold);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                from,
                from + new Vector3(Mathf.Cos(min), Mathf.Sin(min), 0.0f) * this.lookAtDistanceThreshold
                );
            Gizmos.DrawLine(
                from,
                from + new Vector3(Mathf.Cos(max), Mathf.Sin(max), 0.0f) * this.lookAtDistanceThreshold
                );
            Gizmos.color = tempColor;
        }

        private bool CanBeginEquipmentSubject()
        {
            var currentState = this.actor.StateController.CurrentState;

            switch (currentState)
            {
                case ActorStateController.StateType.Movable:
                case ActorStateController.StateType.Guard:
                    return true;
                case ActorStateController.StateType.Attack:
                case ActorStateController.StateType.Avoidance:
                    return this.actor.AnimationParameter.advancedEntry;
                default:
                    return false;
            }
        }

        private bool CanAvoidance()
        {
            var currentState = this.actor.StateController.CurrentState;

            switch (currentState)
            {
                case ActorStateController.StateType.Movable:
                    return true;
                case ActorStateController.StateType.Attack:
                case ActorStateController.StateType.Avoidance:
                    return this.actor.AnimationParameter.advancedEntry;
                default:
                    return false;
            }
        }

        /// <summary>
        /// ロックオン可能な敵を返す
        /// </summary>
        private Actor FindLookAtTargetEnemy()
        {
            Actor result = null;
            var minDistance = float.MaxValue;
            var minAngle = float.MaxValue;
            foreach (var i in Actor.Enemies)
            {
                var diff = i.transform.position - actor.transform.position;
                var distance = diff.magnitude;

                if (distance <= this.lookAtDistanceThreshold && distance <= minDistance)
                {
                    minDistance = distance;

                    var angle = Vector2.Angle(actor.transform.up, diff.normalized);
                    if (angle <= this.lookAtAngleThreshold && angle <= minAngle)
                    {
                        minAngle = angle;
                        result = i;
                    }
                }
            }

            return result;
        }
    }
}
