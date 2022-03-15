using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssetLoader
    {
        public static IObservable<T> LoadAsync<T>(string path)
        {
            return Observable.Defer(() =>
            {
                var handler = Addressables.LoadAssetAsync<T>(path);
                return Observable.FromEvent<AsyncOperationHandle<T>>(
                    x => handler.Completed += x,
                    x => handler.Completed -= x
                    )
                .First()
                .Select(x => x.Result);
            });
        }
    }
}
