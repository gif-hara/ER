using ER.ActorControllers;
using ER.MasterDataSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.StageControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorSpawnerOnSetup : MonoBehaviour, IStageGimmick
    {
        [SerializeField]
        private Actor actorPrefab = default;

        [SerializeField]
        private string enemyDataId = default;

        public void Setup(StageController stageController)
        {
            Assert.IsNotNull(this.actorPrefab, $"{nameof(this.actorPrefab)}がNullです");

            if(stageController.GimmickSpawnManager.CanSpawnEnemy(this.transform, out var id))
            {
                stageController.GimmickSpawnManager.AddSpawnedEnemy(id);
                var t = this.transform;
                this.actorPrefab.Spawn(t.position, t.rotation, EnemyData.Instance.Get(this.enemyDataId).statusData);
            }
        }
    }
}
