using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ER.MasterDataSystem
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "ER/MasterData/ActorDropItem")]
    public sealed class MasterDataActorDropItem : MasterData<MasterDataActorDropItem, MasterDataActorDropItem.Record>
    {
        [Serializable]
        public class Record : IIdHolder<string>
        {
            [SerializeField]
            private string id = default;

            /// <summary>
            /// アイテムを落とすアクターのID
            /// </summary>
            [SerializeField]
            private string actorId = default;

            /// <summary>
            /// アイテムID
            /// </summary>
            [SerializeField]
            private string itemId = default;

            /// <summary>
            /// 落とす確率
            /// </summary>
            [SerializeField]
            private float probability = default;

            public string Id => this.id;

            public Record(string id, string actorId, string itemId, float probability)
            {
                this.id = id;
                this.actorId = actorId;
                this.itemId = itemId;
                this.probability = probability;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Download")]
        private async void Download()
        {
            var task = DownloadFromSpreadSheet("ActorDropItem");
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
            public string ActorId;
            public string ItemId;
            public string Probability;

            public Record ToRecord() => new Record(
                this.Id,
                this.ActorId,
                this.ItemId,
                float.Parse(this.Probability)
                );
        }
#endif
    }
}
