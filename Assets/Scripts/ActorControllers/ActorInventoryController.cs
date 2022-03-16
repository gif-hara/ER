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

        public void AddItem(string itemId)
        {
            //var itemData = ItemData.Instance.
        }
    }
}
