using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnemyLifecycleContoller : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;

        private void Start()
        {
            this.actor.Broker.Receive<ActorEvent.OnDead>()
                .Subscribe(_ =>
                {
                    foreach (var i in Actor.Players)
                    {
                        i.StatusController.AddExperience(this.actor.StatusController.Experience);
                    }
                    Destroy(this.actor.gameObject);
                })
                .AddTo(this);
        }
    }
}
