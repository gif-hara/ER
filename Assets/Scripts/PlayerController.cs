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

        private void Start()
        {
            var inputAction = GameController.Instance.InputAction;

            inputAction.Player.UseRightEquipment.performed += callback =>
            {
                this.actor.Event.OnBeginRightEquipmentSubject().OnNext(Unit.Default);
            };
            inputAction.Player.UseRightEquipment.canceled += callback =>
            {
                this.actor.Event.OnEndRightEquipmentSubject().OnNext(Unit.Default);
            };
            inputAction.Player.UseLeftEquipment.performed += callback =>
            {
                this.actor.Event.OnBeginLeftEquipmentSubject().OnNext(Unit.Default);
            };
            inputAction.Player.UseLeftEquipment.canceled += callback =>
            {
                this.actor.Event.OnEndLeftEquipmentSubject().OnNext(Unit.Default);
            };
            inputAction.Player.Avoidance.performed += callback =>
            {
                if (!this.actor.AnimationParameter.advancedEntry)
                {
                    return;
                }

                var direction = inputAction.Player.Move.ReadValue<Vector2>();
                this.actor.Event.OnRequestAvoidanceSubject().OnNext(direction);
            };
            inputAction.Player.LookAt.performed += callback =>
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

            inputAction.Player.Interact.performed += callback =>
            {
                this.actor.InteractableStageGimmickController.BeginInteract();
            };

            inputAction.Player.OpenIngameMenu.performed += callback =>
            {
                GameEvent.OnRequestOpenIngameMenuSubject().OnNext(Unit.Default);
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
            var inputAction = GameController.Instance.InputAction;
            var t = this.transform;
            var angle = inputAction.Player.Look.ReadValue<Vector2>();
            if (angle.sqrMagnitude > this.angleThreshold * this.angleThreshold)
            {
                this.actor.MotionController.Rotate(angle);
            }

            var direction = inputAction.Player.Move.ReadValue<Vector2>();
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
