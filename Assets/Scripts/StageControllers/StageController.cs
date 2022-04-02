using System;
using ER.ActorControllers;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.StageControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageController : MonoBehaviour
    {
        private Actor actor;

        private Vector2Int currentIndex;

        private StageLoader stageLoader;

        public StageGimmickSpawnManager GimmickSpawnManager { get; } = new StageGimmickSpawnManager();

        private IDisposable disposable = null;

        public StageChunk CurrentStageChunk => this.stageLoader.GetLoadedStateInfo(this.currentIndex).stage;

        private void Awake()
        {
            this.stageLoader = new StageLoader(1, this.transform);

            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
                .Where(x => x.SpawnedActor.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.actor = x.SpawnedActor;
                    this.Load();
                    this.RegisterActorEvent(x.SpawnedActor);
                })
                .AddTo(this);
        }

        private void Update()
        {
            if (this.actor == null)
            {
                return;
            }

            var index = StageLoader.GetIndex(this.actor.transform.position);
            if (this.currentIndex == index)
            {
                return;
            }

            this.currentIndex = index;
            this.Load();
        }

        private void RegisterActorEvent(Actor actor)
        {
            actor.Broker.Receive<ActorEvent.OnInteractedCheckPoint>()
                .Subscribe(_ =>
                {
                    foreach (var i in Actor.Enemies)
                    {
                        Destroy(i.gameObject);
                    }
                    this.GimmickSpawnManager.ResetSpawnedEnemy();
                    this.stageLoader.Clear();
                    this.Load();
                })
                .AddTo(actor.Disposables)
                .AddTo(this);
        }

        private void Load()
        {
            if (this.disposable != null)
            {
                this.disposable.Dispose();
                this.disposable = null;
            }

            this.disposable = this.stageLoader.LoadAsync(this.actor.transform.position)
            .Subscribe(x =>
            {
                foreach (var i in x)
                {
                    if (i.stage == null)
                    {
                        continue;
                    }

                    i.stage.SetupGimmicks(this);
                }
            });
        }
    }
}
