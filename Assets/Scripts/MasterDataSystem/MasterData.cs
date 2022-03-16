using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace ER.MasterDataSystem
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MasterData<T> : ScriptableObject where T : MasterData<T>
    {
        public static T Instance { get; private set; }

        protected virtual void OnSetupped()
        {
        }

        public static IObservable<Unit> SetupAsync(string assetPath)
        {
            return AssetLoader.LoadAsync<T>(assetPath)
                .Do(x =>
                {
                    Instance = x;
                    Instance.OnSetupped();
                })
                .AsUnitObservable();
        }

        protected async Task<string> DownloadFromSpreadSheet(string sheetName)
        {
            var sheetUrl = File.ReadAllText("masterdata_sheet_url.txt");
            var bearer = File.ReadAllText("bearer.txt");
            var request = UnityWebRequest.Get($"{sheetUrl}?mode={sheetName}");
            request.SetRequestHeader("Authorization", $"Bearer {bearer}");
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Delay(100);
            }

            return operation.webRequest.downloadHandler.text;
        }
    }

    public abstract class MasterData
    {
        public static IObservable<Unit> SetupAsync()
        {
            var streams = new List<IObservable<Unit>>();

            return Observable.WhenAll(
                ItemData.SetupAsync("Assets/MasterData/ItemData.asset"),
                WeaponData.SetupAsync("Assets/MasterData/WeaponData.asset")
                )
                .AsUnitObservable();
        }
    }
}
