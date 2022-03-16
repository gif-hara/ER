using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

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