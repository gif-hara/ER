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

        private void Awake()
        {
            this.stageLoader = new StageLoader(1, this.transform);

            GameEvent.OnSpawnedActorSubject()
                .Where(x => x.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.actor = x;
                    this.Load();
                })
                .AddTo(this);
        }

        private void Update()
        {
            if(this.actor == null)
            {
                return;
            }

            var index = StageLoader.GetIndex(this.actor.transform.position);
            if(this.currentIndex == index)
            {
                return;
            }

            this.currentIndex = index;
            this.Load();
        }

        private void Load()
        {
            this.currentIndex = StageLoader.GetIndex(this.actor.transform.position);
            this.stageLoader.LoadAsync(this.actor.transform.position)
            .Subscribe();
        }
    }
}
