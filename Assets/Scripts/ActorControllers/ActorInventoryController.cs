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
        public readonly Dictionary<string, Item> Valuables = new Dictionary<string, Item>();

        public readonly List<Item> Equipments = new List<Item>();

        /// <summary>
        /// 装備品を追加した数
        /// </summary>
        private int addedEquipmentNumber = 0;

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

        private void AddValuables(MasterDataItem.Record masterDataItem, int number)
        {
            Assert.AreEqual(masterDataItem.Category, ItemCategory.Valuable);

            var itemId = masterDataItem.Id;

            if (!this.Valuables.ContainsKey(itemId))
            {
                this.Valuables.Add(itemId, new Item(itemId));
            }

            this.Valuables[itemId].AddNumber(number);
        }

        private void AddEquipments(MasterDataItem.Record masterDataItem, int number)
        {
            Assert.IsTrue(masterDataItem.Category.IsEquipment());

            for (var i = 0; i < number; i++)
            {
                this.Equipments.Add(new Item(masterDataItem.Id, addedEquipmentNumber));
                addedEquipmentNumber++;
            }
        }
    }
}
