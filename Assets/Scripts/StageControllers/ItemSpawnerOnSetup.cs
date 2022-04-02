using ER.ActorControllers;
using I2.Loc;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using System.Collections.Generic;

namespace ER.StageControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ItemSpawnerOnSetup : MonoBehaviour, IStageGimmick
    {
        [SerializeField]
        private List<InteractableStageGimmickItem.Element> elements = default;

        public void Setup(StageController stageController)
        {
            if (stageController.GimmickSpawnManager.CanSpawnItem(this.transform, out var id))
            {
                GameController.Instance.Broker.Publish(GameEvent.OnRequestItemSpawn.Get(
                    this.transform.position,
                    this.elements,
                    () =>
                    {
                        stageController.GimmickSpawnManager.AddSpawnedItem(id);
                    }
                ));
            }
        }
    }
}
