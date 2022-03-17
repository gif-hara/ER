using ER.ActorControllers;
using I2.Loc;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace ER.StageControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ItemSpawnerOnSetup : MonoBehaviour, IStageGimmick
    {
        [SerializeField]
        private InteractableStageGimmickItem itemPrefab = default;

        [SerializeField, TermsPopup]
        private string itemId = default;

        [SerializeField]
        private int number = 1;

        public void Setup(StageController stageController)
        {
            Assert.IsNotNull(this.itemPrefab, $"{nameof(this.itemPrefab)}がNullです");

            if(stageController.GimmickSpawnManager.CanSpawnItem(this.transform, out var id))
            {
                var item = Instantiate(this.itemPrefab, this.transform.position, this.transform.rotation, this.transform);
                item.Setup(stageController);
                item.Setup(this.itemId, this.number);
                item.OnAddedItemAsObservable()
                    .Subscribe(_ =>
                    {
                        stageController.GimmickSpawnManager.AddSpawnedItem(id);
                    })
                    .AddTo(this);
            }
        }
    }
}
