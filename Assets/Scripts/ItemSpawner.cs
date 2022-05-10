using System.Collections.Generic;
using ER.StageControllers;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using UnityEngine.Playables;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private InteractableStageGimmickItem itemPrefab = default;

        [SerializeField]
        private StageController stageController = default;

        void Awake()
        {
            GameController.Instance.Broker.Receive<GameEvent.OnRequestItemSpawn>()
            .Subscribe(x =>
            {
                var (spawnPoint, elements, onAcquiredItemAction) = x;
                var item = Instantiate(this.itemPrefab, spawnPoint, Quaternion.identity, this.stageController.CurrentStageChunk.transform);
                item.Setup(this.stageController);
                item.Setup(elements);
                item.OnAddedItemAsObservable()
                    .Subscribe(_ =>
                    {
                        onAcquiredItemAction?.Invoke();
                    })
                    .AddTo(this);
            })
            .AddTo(this);
        }
    }
}
