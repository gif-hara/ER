using I2.Loc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace ER.MasterDataSystem
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "ER/MasterData/ItemData")]
    public sealed class ItemData : MasterData<ItemData>
    {
        [SerializeField]
        private List<Record> records = default;

        [Serializable]
        public class Record
        {
            [SerializeField, TermsPopup]
            private string id = default;

            /// <summary>
            /// スタック可能か
            /// </summary>
            [SerializeField]
            private bool stackable = default;

            public string Id => this.id;

            public string Name => LocalizationManager.GetTermTranslation(this.id);

            public bool Stackable => this.stackable;
        }

        [ContextMenu("Test")]
        private async void Test()
        {
            // https://www.ka-net.org/blog/?p=12258
            var sheetUrl = File.ReadAllText("masterdata_sheet_url.txt");
            var bearer = File.ReadAllText("bearer.txt");
            var request = UnityWebRequest.Get($"{sheetUrl}?mode=ItemData");
            request.SetRequestHeader("Authorization", $"Bearer {bearer}");
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Delay(100);
            }

            Debug.Log(operation.webRequest.downloadHandler.text);
        }
    }
}
