using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FancyCellInventoryItem
    {
        public string Message { get; }

        public FancyCellInventoryItem(string message)
        {
            this.Message = message;
        }
    }
}
