using ER.MasterDataSystem;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>のインベントリを制御するクラス
    /// </summary>
    public sealed class ActorInventoryController
    {
        private Actor actor;

        /// <summary>
        /// 貴重品
        /// </summary>
        public readonly Dictionary<string, Item> Valuables = new Dictionary<string, Item>();

        /// <summary>
        /// 装備品
        /// </summary>
        public readonly Dictionary<string, Item> Equipments = new Dictionary<string, Item>();

        /// <summary>
        /// 武器のレベルデータ
        /// </summary>
        public readonly Dictionary<string, WeaponLevelData> WeaponLevelDatabase = new Dictionary<string, WeaponLevelData>();

        /// <summary>
        /// 装備品を追加した数
        /// </summary>
        private int addedEquipmentNumber = 0;

        /// <summary>
        /// 利用可能な状態にする
        /// </summary>
        /// <param name="actor"></param>
        public void Setup(Actor actor)
        {
            this.actor = actor;
        }

        /// <summary>
        /// <paramref name="itemId"/>を<paramref name="number"/>の数だけ追加する
        /// </summary>
        public void AddItem(string itemId, int number, bool isShowUI)
        {
            var masterDataItem = MasterDataItem.Get(itemId);
            if (masterDataItem.Category.IsEquipment())
            {
                AddEquipments(masterDataItem, number, isShowUI);
            }
            else if (masterDataItem.Category == ItemCategory.Valuable)
            {
                AddValuables(masterDataItem, number, isShowUI);
            }
            else
            {
                Assert.IsTrue(false, $"{masterDataItem.Category}は未対応です");
            }
        }

        /// <summary>
        /// 装備品を追加する
        /// </summary>
        public Item AddEquipment(MasterDataItem.Record masterDataItem, bool isShowUI)
        {
            Assert.IsTrue(masterDataItem.Category.IsEquipment(), $"{masterDataItem.Id}は装備品ではありません");

            var item = new Item(masterDataItem.Id, addedEquipmentNumber++);
            this.Equipments.Add(item.InstanceId, item);
            if (masterDataItem.Category == ItemCategory.Weapon)
            {
                this.WeaponLevelDatabase.Add(item.InstanceId, new WeaponLevelData());
            }

            this.actor.Broker.Publish(ActorEvent.OnAcquiredItem.Get(masterDataItem, 1, isShowUI));

            return item;
        }

        /// <summary>
        /// 装備品を追加する
        /// </summary>
        public Item AddEquipment(string itemid, bool isShowUI)
        {
            return AddEquipment(MasterDataItem.Get(itemid), isShowUI);
        }

        /// <summary>
        /// 貴重品を追加する
        /// </summary>
        private void AddValuables(MasterDataItem.Record masterDataItem, int number, bool isShowUI)
        {
            Assert.AreEqual(masterDataItem.Category, ItemCategory.Valuable);

            var itemId = masterDataItem.Id;

            if (!this.Valuables.ContainsKey(itemId))
            {
                this.Valuables.Add(itemId, new Item(itemId));
            }

            this.Valuables[itemId].AddNumber(number);

            this.actor.Broker.Publish(ActorEvent.OnAcquiredItem.Get(masterDataItem, number, isShowUI));
        }

        /// <summary>
        /// <paramref name="number"/>の数だけ装備品を追加する
        /// </summary>
        private void AddEquipments(MasterDataItem.Record masterDataItem, int number, bool isShowUI)
        {
            Assert.IsTrue(masterDataItem.Category.IsEquipment(), $"{masterDataItem.Id}は装備品ではありません");

            for (var i = 0; i < number; i++)
            {
                this.AddEquipment(masterDataItem, isShowUI);
            }
        }
        
        /// <summary>
        /// 貴重品の所持数を返す
        /// </summary>
        public int GetValuableNumber(string itemId)
        {
            return !this.Valuables.ContainsKey(itemId) ? 0 : this.Valuables[itemId].Number;
        }
        
        /// <summary>
        /// 攻撃力上昇系の貴重品の所持数を返す
        /// </summary>
        public int GetAttackValuableNumber(AttackAttributeType attackAttributeType)
        {
            return this.GetValuableNumber(ValuableItemUtility.GetAttackId(attackAttributeType));
        }
        
        /// <summary>
        /// 防御力上昇系の貴重品の所持数を返す
        /// </summary>
        public int GetDefenseValuableNumber(AttackAttributeType attackAttributeType)
        {
            return this.GetValuableNumber(ValuableItemUtility.GetDefenseId(attackAttributeType));
        }
    }
}
