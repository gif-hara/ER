using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Item
    {
        private string id;

        /// <summary>
        /// 所持数
        /// </summary>
        private int number;

        public Item(string id)
        {
            this.id = id;
        }

        public void AddNumber(int value)
        {
            this.number += value;
        }
    }
}
