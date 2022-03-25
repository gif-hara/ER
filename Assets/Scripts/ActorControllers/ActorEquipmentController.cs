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
            public EquipmentController CurrentEquipmentController => this.equipmentHolders[this.currentIndex];

            private int currentIndex;

            private EquipmentController[] equipmentHolders = new EquipmentController[Define.EquipmentableNumber];

            private Actor actor;

            public void Setup(Actor actor)
            {
                this.actor = actor;
            }

            /// <summary>
            /// <paramref name="index"/>に対して新規で装備品をアタッチする
            /// </summary>
            public void Attach(int index, EquipmentController equipmentPrefab, string itemInstanceId)
            {
                // もし別のスロットに装備されている場合はスワップする
                for (var i = 0; i < this.equipmentHolders.Length; i++)
                {
                    if (this.equipmentHolders[i] == null)
                    {
                        continue;
                    }
                    if (this.equipmentHolders[i].ItemInstanceId == itemInstanceId)
                    {
                        this.Swap(index, i);
                        return;
                    }
                }

                if (this.equipmentHolders[index] != null)
                {
                    Object.Destroy(this.equipmentHolders[index].gameObject);
                }

                this.equipmentHolders[index] = equipmentPrefab.Attach(this.actor, itemInstanceId);
                this.equipmentHolders[index].gameObject.SetActive(this.currentIndex == index);
            }

            /// <summary>
            /// <paramref name="source"/>と<paramref name="target"/>をスワップする
            /// </summary>
            public void Swap(int source, int target)
            {
                var tempEquipmentController = this.equipmentHolders[source];
                this.equipmentHolders[source] = this.equipmentHolders[target];
                this.equipmentHolders[target] = tempEquipmentController;

                // 今装備しているインデックスとsourceが一致している場合はインデックスも更新する
                if (this.currentIndex == source)
                {
                    this.currentIndex = target;
                }
                else if (this.currentIndex == target)
                {
                    this.currentIndex = source;
                }
            }

            public void Remove(int index)
            {
                var equipmentController = this.equipmentHolders[index];
                if (equipmentController == null)
                {
                    return;
                }

                Assert.AreNotEqual(this.GetEquipmentNumber(), 0, "装備している装備品が0になりました");

                Object.Destroy(equipmentController.gameObject);
                this.equipmentHolders[index] = null;

                // 現在装備中の装備品が削除されたら別の装備品を装備する
                if (this.currentIndex == index)
                {
                    this.ChangeNext();
                }
            }

            public int Find(string itemInstanceId)
            {
                for (var i = 0; i < this.equipmentHolders.Length; i++)
                {
                    if (this.equipmentHolders[i] == null)
                    {
                        continue;
                    }
                    if (this.equipmentHolders[i].ItemInstanceId == itemInstanceId)
                    {
                        return i;
                    }
                }

                return -1;
            }

            public void ChangeNext()
            {
                var oldIndex = this.currentIndex;
                do
                {
                    this.currentIndex = (this.currentIndex + 1) % Define.EquipmentableNumber;
                } while (this.equipmentHolders[this.currentIndex] == null);

                if (oldIndex == currentIndex)
                {
                    return;
                }

                if (this.equipmentHolders[oldIndex] != null)
                {
                    this.equipmentHolders[oldIndex].gameObject.SetActive(false);
                }
                this.equipmentHolders[this.currentIndex].gameObject.SetActive(true);
                this.equipmentHolders[this.currentIndex].PlayDefaultPlayableAsset();
            }

            public EquipmentController GetEquipmentController(int index)
            {
                return this.equipmentHolders[index];
            }

            /// <summary>
            /// 今装備している数を返す
            /// </summary>
            public int GetEquipmentNumber()
            {
                var number = 0;
                foreach (var i in this.equipmentHolders)
                {
                    number += i != null ? 1 : 0;
                }

                return number;
            }
        }
    }
}
