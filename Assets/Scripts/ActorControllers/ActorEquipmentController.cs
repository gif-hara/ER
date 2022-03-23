using ER.EquipmentSystems;
using UnityEngine.Assertions;
using UniRx;
using ER.MasterDataSystem;
using UnityEngine;
using System.Collections.Generic;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorEquipmentController
    {
        private Actor actor;

        public Hand RightHand { get; } = new Hand();

        public Hand LeftHand { get; } = new Hand();

        public EquipmentController GuardingEquipmentController { get; private set; }

        public Item Head { get; private set; }

        public Item Torso { get; private set; }

        public Item Arm { get; private set; }

        public Item Leg { get; private set; }

        /// <summary>
        /// 左手行動のリクエストが来ているか
        /// </summary>
        public bool IsLeftRequest { get; private set; }

        public void Setup(Actor actor)
        {
            this.actor = actor;
            this.RightHand.Setup(actor);
            this.LeftHand.Setup(actor);

            this.actor.Broker.Receive<ActorEvent.BeginEquipment>()
                .Where(x => x.HandType == HandType.Left)
                .Subscribe(_ =>
                {
                    this.IsLeftRequest = true;
                })
                .AddTo(actor.Disposables);

            this.actor.Broker.Receive<ActorEvent.EndEquipment>()
                .Where(x => x.HandType == HandType.Left)
                .Subscribe(_ =>
                {
                    this.IsLeftRequest = false;
                })
                .AddTo(actor.Disposables);

            this.actor.Broker.Receive<ActorEvent.OnRequestChangeEquipment>()
                .Where(x => x.HandType == HandType.Right)
                .Where(_ => this.actor.StateController.CurrentState == ActorStateController.StateType.Movable)
                .Subscribe(_ =>
                {
                    this.RightHand.ChangeNext();
                })
                .AddTo(actor.Disposables);

            this.actor.Broker.Receive<ActorEvent.OnRequestChangeEquipment>()
                .Where(x => x.HandType == HandType.Left)
                .Where(_ => this.actor.StateController.CurrentState == ActorStateController.StateType.Movable)
                .Subscribe(_ =>
                {
                    this.LeftHand.ChangeNext();
                })
                .AddTo(actor.Disposables);
        }

        public void BeginGuard(EquipmentController equipmentController)
        {
            this.GuardingEquipmentController = equipmentController;
        }

        public void EndGuard()
        {
            this.GuardingEquipmentController = null;
        }

        public Item GetArmorItem(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.Head:
                    return this.Head;
                case ArmorType.Torso:
                    return this.Torso;
                case ArmorType.Arm:
                    return this.Arm;
                case ArmorType.Leg:
                    return this.Leg;
                default:
                    Assert.IsTrue(false, $"{armorType}は未対応です");
                    return null;
            }
        }

        public void SetArmorItem(ArmorType armorType, string itemInstanceId)
        {
            switch (armorType)
            {
                case ArmorType.Head:
                    this.Head = this.actor.InventoryController.Equipments[itemInstanceId]; ;
                    break;
                case ArmorType.Torso:
                    this.Torso = this.actor.InventoryController.Equipments[itemInstanceId]; ;
                    break;
                case ArmorType.Arm:
                    this.Arm = this.actor.InventoryController.Equipments[itemInstanceId]; ;
                    break;
                case ArmorType.Leg:
                    this.Leg = this.actor.InventoryController.Equipments[itemInstanceId]; ;
                    break;
                default:
                    Assert.IsTrue(false, $"{armorType}は未対応です");
                    break;
            }
        }

        public int GetDefense(AttackAttributeType attackAttributeType)
        {
            var result = 0;
            if (this.Head != null) result += this.Head.MasterDataItem.ToArmor().GetDefense(attackAttributeType);
            if (this.Torso != null) result += this.Torso.MasterDataItem.ToArmor().GetDefense(attackAttributeType);
            if (this.Arm != null) result += this.Arm.MasterDataItem.ToArmor().GetDefense(attackAttributeType);
            if (this.Leg != null) result += this.Leg.MasterDataItem.ToArmor().GetDefense(attackAttributeType);

            return result;
        }

        public EquipmentController GetEquipmentController(HandType handType)
        {
            switch (handType)
            {
                case HandType.Left:
                    return this.LeftHand.CurrentEquipmentController;
                case HandType.Right:
                    return this.RightHand.CurrentEquipmentController;
                default:
                    Assert.IsTrue(false, $"{handType}は未実装です");
                    return null;
            }
        }

        public class Hand
        {
            public EquipmentController CurrentEquipmentController => this.equipmentHolders[this.index];

            private int index;

            private EquipmentController[] equipmentHolders = new EquipmentController[Define.EquipmentableNumber];

            private Actor actor;

            public void Setup(Actor actor)
            {
                this.actor = actor;
            }

            public void Attach(int index, EquipmentController equipmentPrefab, string itemInstanceId)
            {
                if (this.equipmentHolders[index] != null)
                {
                    Object.Destroy(this.equipmentHolders[index].gameObject);
                }

                this.equipmentHolders[index] = equipmentPrefab.Attach(this.actor, itemInstanceId);
                this.equipmentHolders[index].gameObject.SetActive(this.index == index);
            }

            public void ChangeNext()
            {
                var oldIndex = this.index;
                do
                {
                    this.index = (this.index + 1) % Define.EquipmentableNumber;
                } while (this.equipmentHolders[this.index] == null);

                if (oldIndex == index)
                {
                    return;
                }

                this.equipmentHolders[oldIndex].gameObject.SetActive(false);
                this.equipmentHolders[this.index].gameObject.SetActive(true);
                this.equipmentHolders[this.index].PlayDefaultPlayableAsset();
            }

            public EquipmentController GetEquipmentController(int index)
            {
                return this.equipmentHolders[index];
            }
        }
    }
}
