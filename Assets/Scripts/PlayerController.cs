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
        private List<IEquipmentSelector> rightEquipmentSelectors = default;

        [SerializeReference, SubclassSelector(typeof(IEquipmentSelector))]
        private List<IEquipmentSelector> leftEquipmentSelectors = default;

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
                this.actor.Broker.Publish(ActorEvent.BeginEquipment.Get(HandType.Right));
            };
            inputAction.Player.UseRightEquipment.canceled += callback =>
            {
                this.actor.Broker.Publish(ActorEvent.EndEquipment.Get(HandType.Right));
            };
            inputAction.Player.UseLeftEquipment.performed += callback =>
            {
                this.actor.Broker.Publish(ActorEvent.BeginEquipment.Get(HandType.Left));
            };
            inputAction.Player.UseLeftEquipment.canceled += callback =>
            {
                this.actor.Broker.Publish(ActorEvent.EndEquipment.Get(HandType.Left));
            };
            inputAction.Player.Avoidance.performed += callback =>
            {
                if (!this.actor.AnimationParameter.advancedEntry)
                {
                    return;
                }

                var direction = inputAction.Player.Move.ReadValue<Vector2>();
                this.actor.Broker.Publish(ActorEvent.OnRequestAvoidance.Get(direction));
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
                GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenIngameMenu.Get());
            };

            inputAction.Player.ChangeRightEquipment.performed += callback =>
            {
                this.actor.Broker.Publish(ActorEvent.OnRequestChangeEquipment.Get(HandType.Right));
            };

            inputAction.Player.ChangeLeftEquipment.performed += callback =>
            {
                this.actor.Broker.Publish(ActorEvent.OnRequestChangeEquipment.Get(HandType.Left));
            };

            for (var i = 0; i < this.rightEquipmentSelectors.Count; i++)
            {
                this.rightEquipmentSelectors[i].Attach(this.actor, i);
            }

            for (var i = 0; i < this.leftEquipmentSelectors.Count; i++)
            {
                this.leftEquipmentSelectors[i].Attach(this.actor, i);
            }

            var headItem = this.actor.InventoryController.AddEquipment(this.headMasterDataId);
            var torsoItem = this.actor.InventoryController.AddEquipment(this.torsoMasterDataId);
            var armItem = this.actor.InventoryController.AddEquipment(this.armMasterDataId);
            var legItem = this.actor.InventoryController.AddEquipment(this.legMasterDataId);

            this.actor.EquipmentController.SetArmorItem(ArmorType.Head, headItem.InstanceId);
            this.actor.EquipmentController.SetArmorItem(ArmorType.Torso, torsoItem.InstanceId);
            this.actor.EquipmentController.SetArmorItem(ArmorType.Arm, armItem.InstanceId);
            this.actor.EquipmentController.SetArmorItem(ArmorType.Leg, legItem.InstanceId);
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
