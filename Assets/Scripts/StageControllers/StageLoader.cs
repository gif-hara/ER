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

        public const int SplitSize = 40;

        /// <summary>
        /// 読み込む周りのステージの数
        /// </summary>
        /// <remarks>
        /// e.g.) 1の場合・・・
        /// ooo
        /// oxo
        /// ooo
        /// 
        /// e.g.) 2の場合・・・
        /// ooooo
        /// ooooo
        /// ooxoo
        /// ooooo
        /// ooooo
        /// </remarks>
        private int range;

        /// <summary>
        /// ステージの親オブジェクト
        /// </summary>
        private Transform stageParent;

        public StageLoader(int range, Transform stageParent)
        {
            this.range = range;
            this.stageParent = stageParent;
        }

        /// <summary>
        /// 非同期で読み込みを開始する
        /// </summary>
        /// <remarks>
        /// 返り値には新規で生成した<see cref="StageInfo"/>のリストを返す
        /// </remarks>
        public IObservable<StageInfo[]> LoadAsync(Vector3 position)
        {
            return Observable.Defer(() =>
            {
                loadRequestIndexies.Clear();
                var centerX = Mathf.FloorToInt(position.x / SplitSize);
                var centerY = Mathf.FloorToInt(position.y / SplitSize);

                for (var y = centerY - this.range; y <= centerY + this.range; y++)
                {
                    for (var x = centerX - this.range; x <= centerX + this.range; x++)
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
                        UnityEngine.Object.Destroy(i.stage.gameObject);
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

                var loadStreams = new List<IObservable<StageInfo>>();
                foreach (var i in this.loadRequestIndexies)
                {
                    var stageInfo = i;
                    var index = i.index;
                    var stream = AssetLoader.LoadAsync<GameObject>($"Assets/Prefabs/Stage.Chunk({index.x},{index.y},0).prefab")
                    .Do(x =>
                    {
                        stageInfo.stage = UnityEngine.Object.Instantiate(x, this.stageParent).GetComponent<StageChunk>();
                        stageInfo.stage.transform.localPosition = new Vector3(index.x * SplitSize, index.y * SplitSize, 0);
                        this.loadedIndexies.Add(stageInfo);
                    })
                    .Select(_ => stageInfo);
                    loadStreams.Add(stream);
                }

                return Observable.WhenAll(loadStreams);
            });
        }

        public void Clear()
        {
            foreach (var i in this.loadedIndexies)
            {
                UnityEngine.Object.Destroy(i.stage.gameObject);
            }
            this.loadedIndexies.Clear();
        }

        public static Vector2Int GetIndex(Vector3 position)
        {
            return new Vector2Int(Mathf.FloorToInt(position.x / SplitSize), Mathf.FloorToInt(position.y / SplitSize));
        }

        public struct StageInfo : IEquatable<StageInfo>
        {
            public Vector2Int index;

            public StageChunk stage;

            public bool Equals(StageInfo other)
            {
                return this.index == other.index;
            }
        }
    }
}
