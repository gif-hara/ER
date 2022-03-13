using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.StageControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageLoader
    {
        private readonly List<StageInfo> loadedIndexies = new List<StageInfo>();

        private readonly List<StageInfo> loadRequestIndexies = new List<StageInfo>();

        private readonly List<StageInfo> removeRequestIndexies = new List<StageInfo>();

        public const int SplitSize = 20;

        public IObservable<Unit> LoadAsync(Vector3 position)
        {
            return Observable.Defer(() =>
            {
                loadRequestIndexies.Clear();
                var centerX = Mathf.FloorToInt(position.x / SplitSize);
                var centerY = Mathf.FloorToInt(position.y / SplitSize);

                for (var y = centerY - 1; y <= centerY + 1; y++)
                {
                    for (var x = centerX - 1; x <= centerX + 1; x++)
                    {
                        loadRequestIndexies.Add(new StageInfo
                        {
                            index = new Vector2Int(x, y)
                        });
                    }
                }

                // 不要となったものを削除
                {
                    this.removeRequestIndexies.Clear();

                    foreach (var i in this.loadedIndexies)
                    {
                        if (!this.loadRequestIndexies.Contains(i))
                        {
                            this.removeRequestIndexies.Add(i);
                        }
                    }

                    foreach (var i in this.removeRequestIndexies)
                    {
                        UnityEngine.Object.Destroy(i.stage);
                        this.loadedIndexies.Remove(i);
                    }
                }

                // 新規で必要になったものを読み込む
                {
                    this.removeRequestIndexies.Clear();
                    foreach (var i in this.loadRequestIndexies)
                    {
                        if (this.loadedIndexies.Contains(i))
                        {
                            this.removeRequestIndexies.Add(i);
                        }
                    }

                    foreach (var i in this.removeRequestIndexies)
                    {
                        this.loadRequestIndexies.Remove(i);
                    }
                }

                var loadStreams = new List<IObservable<GameObject>>();
                foreach (var i in this.loadRequestIndexies)
                {
                    var stageInfo = i;
                    var index = i.index;
                    var handler = Addressables.LoadAssetAsync<GameObject>($"Assets/Prefabs/Stage.Chunk({index.x},{index.y},0).prefab");
                    var stream = Observable.FromEvent<AsyncOperationHandle<GameObject>>(
                        x => handler.Completed += x,
                        x => handler.Completed -= x
                        )
                    .First()
                    .Select(x => x.Result)
                    .Do(x =>
                    {
                        stageInfo.stage = UnityEngine.Object.Instantiate(x);
                        stageInfo.stage.transform.localPosition = new Vector3(index.x * SplitSize * 2, index.y * SplitSize * 2, 0);
                    });
                    loadStreams.Add(stream);
                }

                return Observable.WhenAll(loadStreams).AsUnitObservable();
            });
        }

        private struct StageInfo : IEquatable<StageInfo>
        {
            public Vector2Int index;

            public GameObject stage;

            public bool Equals(StageInfo other)
            {
                return this.index == other.index;
            }
        }
    }
}
