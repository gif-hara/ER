using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
#endif

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

        public static IObservable<bool> IsExists(string path)
        {
            return Observable.Defer(() =>
            {
#if UNITY_EDITOR
                // Fast Modeの場合はAssetDatabaseを使わないとNullが出る
                if (GetSettings().ActivePlayModeDataBuilderIndex == 0)
                {
                    return Observable.Return(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) != null);
                }
#endif
                var handler = Addressables.LoadResourceLocationsAsync(path);
                return Observable.FromEvent<AsyncOperationHandle<IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>>>(
                    x => handler.Completed += x,
                    x => handler.Completed -= x
                    )
                .First()
                .Select(x =>
                {
                    return x.Status == AsyncOperationStatus.Succeeded && x.Result != null && x.Result.Count > 0;
                });
            });
        }

#if UNITY_EDITOR

        private static AddressableAssetSettings settings;

        private static AddressableAssetSettings GetSettings()
        {
            if (settings != null)
            {
                return settings;
            }

            settings = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
            return settings;
        }

#endif
    }
}
