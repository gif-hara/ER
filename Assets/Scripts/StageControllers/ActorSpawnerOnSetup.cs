using ER.ActorControllers;
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

        public void Setup(StageController stageController)
        {
            Assert.IsNotNull(this.actorPrefab, $"{nameof(this.actorPrefab)}がNullです");

            string id;
            if(stageController.GimmickSpawnManager.CanSpawnEnemy(this.transform, out id))
            {
                stageController.GimmickSpawnManager.AddSpawnedEnemy(id);
                Instantiate(this.actorPrefab, this.transform.position, this.transform.rotation);
            }
        }
    }
}
