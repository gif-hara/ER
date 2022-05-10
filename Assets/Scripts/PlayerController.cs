using ER.ActorControllers;
using ER.EquipmentSystems;
using I2.Loc;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

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

        [SerializeField, TermsPopup("Weapon/")]
        private List<string> addWeapons = default;

        [SerializeField, TermsPopup("Shield/")]
        private List<string> addShields = default;

        [SerializeField, TermsPopup("Armor")]
        public List<string> addArmors = default;

        private void Start()
        {
            var inputAction = GameController.Instance.InputAction.Player;

            inputAction.UseRightEquipment.OnPerformedAsObservable()
                .Subscribe(_ =>
                {
                    this.actor.Broker.Publish(ActorEvent.BeginEquipment.Get(HandType.Right));
                })
                .AddTo(this);

            inputAction.UseRightEquipment.OnCanceledAsObservable()
                .Subscribe(_ =>
                {
                    this.actor.Broker.Publish(ActorEvent.EndEquipment.Get(HandType.Right));
                })
                .AddTo(this);

            inputAction.UseLeftEquipment.OnPerformedAsObservable()
                .Subscribe(_ =>
                {
                    this.actor.Broker.Publish(ActorEvent.BeginEquipment.Get(HandType.Left));
                })
                .AddTo(this);

            inputAction.UseLeftEquipment.OnCanceledAsObservable()
                .Subscribe(_ =>
                {
                    this.actor.Broker.Publish(ActorEvent.EndEquipment.Get(HandType.Left));
                })
                .AddTo(this);

            inputAction.Avoidance.OnPerformedAsObservable()
                .Subscribe(_ =>
                {
                    if (!this.actor.AnimationParameter.advancedEntry)
                    {
                        return;
                    }

                    var direction = inputAction.Move.ReadValue<Vector2>();
                    this.actor.Broker.Publish(ActorEvent.OnRequestAvoidance.Get(direction));
                })
                .AddTo(this);

            inputAction.LookAt.OnPerformedAsObservable()
                .Subscribe(_ =>
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
                })
                .AddTo(this);

            inputAction.Interact.OnPerformedAsObservable()
                .Subscribe(_ =>
                {
                    this.actor.InteractableStageGimmickController.BeginInteract();
                })
                .AddTo(this);

            inputAction.OpenIngameMenu.OnPerformedAsObservable()
                .Subscribe(_ =>
                {
                    GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenIngameMenu.Get(IngameMenuType.FromStartButton));
                })
                .AddTo(this);

            inputAction.ChangeRightEquipment.OnPerformedAsObservable()
                .Subscribe(_ =>
                {
                    this.actor.Broker.Publish(ActorEvent.OnRequestChangeEquipment.Get(HandType.Right));
                })
                .AddTo(this);

            inputAction.ChangeLeftEquipment.OnPerformedAsObservable()
                .Subscribe(_ =>
                {
                    this.actor.Broker.Publish(ActorEvent.OnRequestChangeEquipment.Get(HandType.Left));
                })
                .AddTo(this);

            inputAction.UseItem.OnPerformedAsObservable()
                .Subscribe(_ =>
                {
                    if (!this.actor.AnimationParameter.advancedEntry)
                    {
                        return;
                    }

                    this.actor.Broker.Publish(ActorEvent.OnRequestStartRecoveryItem.Get());
                })
                .AddTo(this);

            inputAction.Select.OnPerformedAsObservable()
                .Subscribe(_ =>
                {
                    GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenInputTutorial.Get());
                })
                .AddTo(this);

            for (var i = 0; i < this.rightEquipmentSelectors.Count; i++)
            {
                this.rightEquipmentSelectors[i].Attach(this.actor, i);
            }

            for (var i = 0; i < this.leftEquipmentSelectors.Count; i++)
            {
                this.leftEquipmentSelectors[i].Attach(this.actor, i);
            }

            var headItem = this.actor.InventoryController.AddEquipment(this.headMasterDataId, false);
            var torsoItem = this.actor.InventoryController.AddEquipment(this.torsoMasterDataId, false);
            var armItem = this.actor.InventoryController.AddEquipment(this.armMasterDataId, false);
            var legItem = this.actor.InventoryController.AddEquipment(this.legMasterDataId, false);

            this.actor.EquipmentController.SetArmorItem(ArmorType.Head, headItem.InstanceId);
            this.actor.EquipmentController.SetArmorItem(ArmorType.Torso, torsoItem.InstanceId);
            this.actor.EquipmentController.SetArmorItem(ArmorType.Arm, armItem.InstanceId);
            this.actor.EquipmentController.SetArmorItem(ArmorType.Leg, legItem.InstanceId);

            foreach (var i in this.addWeapons)
            {
                this.actor.InventoryController.AddEquipment(i, false);
            }

            foreach (var i in this.addShields)
            {
                this.actor.InventoryController.AddEquipment(i, false);
            }

            foreach (var i in this.addArmors)
            {
                this.actor.InventoryController.AddEquipment(i, false);
            }
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
