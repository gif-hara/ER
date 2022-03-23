using ER.MasterDataSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Item
    {
        public string ItemId { get; private set; }

        public string InstanceId { get; private set; }

        /// <summary>
        /// 所持数
        /// </summary>
        public int Number { get; private set; }

        public MasterDataItem.Record MasterDataItem => MasterDataSystem.MasterDataItem.Get(this.ItemId);

        public Item(string id)
        {
            this.ItemId = id;
        }

        public Item(string id, int instanceId)
        {
            this.ItemId = id;
            this.InstanceId = id + instanceId.ToString();
        }

        public void AddNumber(int value)
        {
            this.Number += value;
        }
    }
}
