using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Item
    {
        private string itemId;

        private string instanceId;

        /// <summary>
        /// 所持数
        /// </summary>
        private int number;

        public Item(string id)
        {
            this.itemId = id;
        }

        public Item(string id, int instanceId)
        {
            this.itemId = id;
            this.instanceId = id + instanceId.ToString();
        }

        public void AddNumber(int value)
        {
            this.number += value;
        }
    }
}
