using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;
using UniRx;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorRecoveryItemController : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector recoveryItemPrefab = default;

        public void Setup(Actor actor)
        {
            actor.Broker.Receive<ActorEvent.OnRequestStartRecoveryItem>()
                .Where(_ => actor.StatusController.CanUseRecoveryItem())
                .Subscribe(_ =>
                {
                    var instance = Instantiate(this.recoveryItemPrefab, actor.transform);
                    instance.SetGenericBinding("ActorAnimation", actor.Animator);
                    instance.Play();
                    instance.OnStoppedAsObservable()
                    .Subscribe(__ =>
                    {
                        Destroy(instance.gameObject);
                        actor.StateController.ChangeRequest(ActorStateController.StateType.Movable);
                    })
                    .AddTo(instance);

                    actor.StateController.ChangeRequest(ActorStateController.StateType.UseRecoveryItem);
                })
                .AddTo(actor.Disposables)
                .AddTo(this);
        }
    }
}
