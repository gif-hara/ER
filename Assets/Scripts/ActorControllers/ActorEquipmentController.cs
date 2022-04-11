using ER.EquipmentSystems;
using UnityEngine.Assertions;
using UniRx;
using UnityEngine;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>の装備品を制御するクラス
    /// </summary>
    public sealed class ActorEquipmentController
    {
        private Actor actor;

        /// <summary>
        /// 右手の装備品
        /// </summary>
        public Hand RightHand { get; } = new Hand();

        /// <summary>
        /// 左手の装備品
        /// </summary>
        public Hand LeftHand { get; } = new Hand();
        
        /// <summary>
        /// 攻撃を行っている装備品
        /// </summary>
        public EquipmentController AttackingEquipmentController { get; private set; }
        
        /// <summary>
        /// ガードを行っている装備品
        /// </summary>
        public EquipmentController GuardingEquipmentController { get; private set; }

        /// <summary>
        /// 頭防具
        /// </summary>
        public Item Head { get; private set; }

        /// <summary>
        /// 胴防具
        /// </summary>
        public Item Torso { get; private set; }

        /// <summary>
        /// 腕防具
        /// </summary>
        public Item Arm { get; private set; }

        /// <summary>
        /// 足防具
        /// </summary>
        public Item Leg { get; private set; }

        /// <summary>
        /// 左手行動のリクエストが来ているか
        /// </summary>
        public bool IsLeftRequest { get; private set; }

        /// <summary>
        /// 利用可能な状態にする
        /// </summary>
        public void Setup(Actor actor)
        {
            this.actor = actor;
            this.RightHand.Setup(actor, HandType.Right);
            this.LeftHand.Setup(actor, HandType.Left);

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

        /// <summary>
        /// 攻撃を開始する
        /// </summary>
        public void BeginAttack(EquipmentController equipmentController)
        {
            this.AttackingEquipmentController = equipmentController;
        }
        
        /// <summary>
        /// 攻撃を終了する
        /// </summary>
        public void EndAttack()
        {
            this.AttackingEquipmentController = null;
        }

        /// <summary>
        /// ガードを開始する
        /// </summary>
        public void BeginGuard(EquipmentController equipmentController)
        {
            this.GuardingEquipmentController = equipmentController;
        }

        /// <summary>
        /// ガードを終了する
        /// </summary>
        public void EndGuard()
        {
            this.GuardingEquipmentController = null;
        }

        /// <summary>
        /// <paramref name="armorType"/>に対応した防具を返す
        /// </summary>
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

        /// <summary>
        /// <paramref name="armorType"/>に対応した防具を設定する
        /// </summary>
        public void SetArmorItem(ArmorType armorType, string itemInstanceId)
        {
            switch (armorType)
            {
                case ArmorType.Head:
                    this.Head = this.actor.InventoryController.Equipments[itemInstanceId];
                    break;
                case ArmorType.Torso:
                    this.Torso = this.actor.InventoryController.Equipments[itemInstanceId];
                    break;
                case ArmorType.Arm:
                    this.Arm = this.actor.InventoryController.Equipments[itemInstanceId];
                    break;
                case ArmorType.Leg:
                    this.Leg = this.actor.InventoryController.Equipments[itemInstanceId];
                    break;
                default:
                    Assert.IsTrue(false, $"{armorType}は未対応です");
                    break;
            }
        }

        /// <summary>
        /// <paramref name="armorType"/>に対応する防具を外す
        /// </summary>
        public void RemoveArmorItem(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.Head:
                    this.Head = null;
                    break;
                case ArmorType.Torso:
                    this.Torso = null;
                    break;
                case ArmorType.Arm:
                    this.Arm = null;
                    break;
                case ArmorType.Leg:
                    this.Leg = null;
                    break;
                default:
                    Assert.IsTrue(false, $"{armorType}は未対応です");
                    break;
            }
        }

        /// <summary>
        /// <paramref name="attackAttributeType"/>の総合防御力を返す
        /// </summary>
        public int GetDefense(AttackAttributeType attackAttributeType)
        {
            var result = 0;
            if (this.Head != null) result += this.Head.MasterDataItem.ToArmor().GetDefense(attackAttributeType);
            if (this.Torso != null) result += this.Torso.MasterDataItem.ToArmor().GetDefense(attackAttributeType);
            if (this.Arm != null) result += this.Arm.MasterDataItem.ToArmor().GetDefense(attackAttributeType);
            if (this.Leg != null) result += this.Leg.MasterDataItem.ToArmor().GetDefense(attackAttributeType);

            return result;
        }

        /// <summary>
        /// <paramref name="handType"/>に対応する装備品を返す
        /// </summary>
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

        /// <summary>
        /// 手
        /// </summary>
        public class Hand
        {
            /// <summary>
            /// 現在利用している装備品を返す
            /// </summary>
            public EquipmentController CurrentEquipmentController => this.equipmentHolders[this.currentIndex];

            private int currentIndex;

            /// <summary>
            /// 装備品ホルダー
            /// </summary>
            private EquipmentController[] equipmentHolders = new EquipmentController[Define.EquipmentableNumber];

            private Actor actor;

            private HandType handType;

            /// <summary>
            /// 利用可能な状態にする
            /// </summary>
            public void Setup(Actor actor, HandType handType)
            {
                this.actor = actor;
                this.handType = handType;
            }

            /// <summary>
            /// <paramref name="index"/>に対して新規で装備品をアタッチする
            /// </summary>
            public void Attach(int index, EquipmentController equipmentPrefab, string itemInstanceId)
            {
                Assert.AreEqual(this.Find(itemInstanceId), -1, "Swapしてください");

                if (this.equipmentHolders[index] != null)
                {
                    Object.Destroy(this.equipmentHolders[index].gameObject);
                }

                this.equipmentHolders[index] = equipmentPrefab.Attach(this.actor, itemInstanceId, this.handType);
                this.equipmentHolders[index].gameObject.SetActive(this.currentIndex == index);

                if (this.currentIndex == index)
                {
                    this.actor.Broker.Publish(ActorEvent.OnChangedHandEquipment.Get(this.handType, this.equipmentHolders[this.currentIndex]));
                }
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
                    this.actor.Broker.Publish(ActorEvent.OnChangedHandEquipment.Get(this.handType, this.equipmentHolders[this.currentIndex]));
                }
                else if (this.currentIndex == target)
                {
                    this.currentIndex = source;
                    this.actor.Broker.Publish(ActorEvent.OnChangedHandEquipment.Get(this.handType, this.equipmentHolders[this.currentIndex]));
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

                this.actor.Broker.Publish(ActorEvent.OnChangedHandEquipment.Get(this.handType, this.equipmentHolders[this.currentIndex]));
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
