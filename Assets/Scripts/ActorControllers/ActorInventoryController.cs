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
        public readonly Dictionary<string, Item> Inventory = new Dictionary<string, Item>();

        public void AddItem(string itemId, int number)
        {
            if(!this.Inventory.ContainsKey(itemId))
            {
                this.Inventory.Add(itemId, new Item(itemId));
            }

            this.Inventory[itemId].AddNumber(number);
            Debug.Log("AddItem!");
        }
    }
}
