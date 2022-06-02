using System.Collections.Generic;
using ER.ActorControllers;
using ER.ERBehaviour;
using ER.MasterDataSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace ER.StageControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorSpawnerOnSetup : MonoBehaviour, IStageGimmick
    {
        [SerializeField]
        private string actorStatusId = default;

        [SerializeReference, SubclassSelector(typeof(IAction))]
        private List<IAction> onDeadActions;

        public void Setup(StageController stageController)
        {
            if (stageController.GimmickSpawnManager.CanSpawnEnemy(this.transform, out var id))
            {
                stageController.GimmickSpawnManager.AddSpawnedEnemy(id);
                var t = this.transform;
                var masterDataActorStatus = MasterDataActorStatus.Get(this.actorStatusId);
                var actor = masterDataActorStatus.actorPrefab.Spawn(
                    t.position,
                    t.rotation,
                    actorStatusId
                    );

                if (this.onDeadActions.Count > 0)
                {
                    actor.Broker.Receive<ActorEvent.OnDead>()
                        .Subscribe(_ =>
                        {
                            var actorHolder = new ActorHolderBehaviourData();
                            foreach (var onDeadAction in this.onDeadActions)
                            {
                                onDeadAction.Invoke(actorHolder);
                            }
                        });
                }
            }
        }
    }
}
