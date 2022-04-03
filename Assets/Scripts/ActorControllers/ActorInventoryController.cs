using ER.MasterDataSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorInventoryController
    {
        private Actor actor;

        public readonly Dictionary<string, Item> Valuables = new Dictionary<string, Item>();

        public readonly Dictionary<string, Item> Equipments = new Dictionary<string, Item>();

        /// <summary>
        /// 武器のレベルデータ
        /// </summary>
        public readonly Dictionary<string, WeaponLevelData> WeaponLevelDatabase = new Dictionary<string, WeaponLevelData>();

        /// <summary>
        /// 装備品を追加した数
        /// </summary>
        private int addedEquipmentNumber = 0;

        public void Setup(Actor actor)
        {
            this.actor = actor;
        }

        public void AddItem(string itemId, int number)
        {
            var masterDataItem = MasterDataItem.Get(itemId);
            if (masterDataItem.Category.IsEquipment())
            {
                AddEquipments(masterDataItem, number);
            }
            else if (masterDataItem.Category == ItemCategory.Valuable)
            {
                AddValuables(masterDataItem, number);
            }
            else
            {
                Assert.IsTrue(false, $"{masterDataItem.Category}は未対応です");
            }
        }

        public Item AddEquipment(MasterDataItem.Record masterDataItem)
        {
            Assert.IsTrue(masterDataItem.Category.IsEquipment(), $"{masterDataItem.Id}は装備品ではありません");

            var item = new Item(masterDataItem.Id, addedEquipmentNumber++);
            this.Equipments.Add(item.InstanceId, item);
            if (masterDataItem.Category == ItemCategory.Weapon)
            {
                this.WeaponLevelDatabase.Add(item.InstanceId, new WeaponLevelData());
            }

            this.actor.Broker.Publish(ActorEvent.OnAcquiredItem.Get(masterDataItem, 1));

            return item;
        }

        public Item AddEquipment(string itemid)
        {
            return AddEquipment(MasterDataItem.Get(itemid));
        }

        private void AddValuables(MasterDataItem.Record masterDataItem, int number)
        {
            Assert.AreEqual(masterDataItem.Category, ItemCategory.Valuable);

            var itemId = masterDataItem.Id;

            if (!this.Valuables.ContainsKey(itemId))
            {
                this.Valuables.Add(itemId, new Item(itemId));
            }

            this.Valuables[itemId].AddNumber(number);

            this.actor.Broker.Publish(ActorEvent.OnAcquiredItem.Get(masterDataItem, number));
        }

        private void AddEquipments(MasterDataItem.Record masterDataItem, int number)
        {
            Assert.IsTrue(masterDataItem.Category.IsEquipment(), $"{masterDataItem.Id}は装備品ではありません");

            for (var i = 0; i < number; i++)
            {
                this.AddEquipment(masterDataItem);
            }
        }
    }
}
