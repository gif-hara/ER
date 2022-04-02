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
        private string actorStatusId = default;

        public void Setup(StageController stageController)
        {
            if (stageController.GimmickSpawnManager.CanSpawnEnemy(this.transform, out var id))
            {
                stageController.GimmickSpawnManager.AddSpawnedEnemy(id);
                var t = this.transform;
                var masterDataActorStatus = MasterDataActorStatus.Get(this.actorStatusId);
                masterDataActorStatus.actorPrefab.Spawn(
                    t.position,
                    t.rotation,
                    actorStatusId
                    );
            }
        }
    }
}
