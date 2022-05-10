using ER.ActorControllers;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx.Triggers;
using UniRx;

namespace ER.StageControllers
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class InteractableStageGimmick : MonoBehaviour, IInteractableStageGimmick
    {
        public abstract void BeginInteract(Actor actor);
        
        public abstract string LocalizedNavigationMessage
        {
            get;
        }

        public void Setup(StageController stageController)
        {
            this.OnTriggerEnter2DAsObservable()
                .Select(x => x.attachedRigidbody.GetComponent<Actor>())
                .Where(x => x != null)
                .Subscribe(x => x.Broker.Publish(ActorEvent.OnEnterInteractableStageGimmick.Get(this)))
                .AddTo(this);

            this.OnTriggerExit2DAsObservable()
                .Select(x => x.attachedRigidbody.GetComponent<Actor>())
                .Where(x => x != null)
                .Subscribe(x => x.Broker.Publish(ActorEvent.OnExitInteractableStageGimmick.Get(this)))
                .AddTo(this);
        }
    }
}
