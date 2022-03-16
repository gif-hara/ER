using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ER.MasterDataSystem
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "ER/MasterData/ItemData")]
    public sealed class ItemData : MasterData<ItemData, ItemData.Record>
    {
        [Serializable]
        public class Record : IIdHolder<string>
        {
            [SerializeField, TermsPopup]
            private string id = default;

            /// <summary>
            /// スタック可能か
            /// </summary>
            [SerializeField]
            private bool stackable = default;

            [SerializeField]
            private ItemCategory category = default;

            public string Id => this.id;

            public string Name => LocalizationManager.GetTermTranslation(this.id);

            public bool Stackable => this.stackable;

            public ItemCategory Category => this.category;

            public Record(string id, bool stackable, ItemCategory category)
            {
                this.id = id;
                this.stackable = stackable;
                this.category = category;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Download")]
        private async void Download()
        {
            var task = DownloadFromSpreadSheet(nameof(ItemData));
            await task;

            var result = JsonUtility.FromJson<Json>(task.Result);

            this.records = result.elements.Select(x => x.ToRecord()).ToList();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [Serializable]
        private class Json
        {
            public List<JsonElement> elements;
        }

        [Serializable]
        private class JsonElement
        {
            public string Id;
            public string Stackable;
            public string Category;

            public Record ToRecord() => new Record(
                $"Item/{this.Id}",
                bool.Parse(this.Stackable),
                (ItemCategory)Enum.Parse(typeof(ItemCategory), this.Category)
                );
        }
#endif
    }
}
