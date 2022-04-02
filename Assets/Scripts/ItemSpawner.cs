using System.Collections.Generic;
using ER.StageControllers;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

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
                var item = Instantiate(this.itemPrefab, x.SpawnPoint, Quaternion.identity, this.stageController.CurrentStageChunk.transform);
                item.Setup(this.stageController);
                item.Setup(x.Elements);
                item.OnAddedItemAsObservable()
                    .Subscribe(_ =>
                    {
                        x.OnAcquiredItemAction?.Invoke();
                    })
                    .AddTo(this);
            })
            .AddTo(this);
        }
    }
}
