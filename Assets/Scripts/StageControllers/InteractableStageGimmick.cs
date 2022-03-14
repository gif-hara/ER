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

        public void Setup(StageController stageController)
        {
            this.OnTriggerEnter2DAsObservable()
                .Select(x => x.attachedRigidbody.GetComponent<Actor>())
                .Where(x => x != null)
                .Subscribe(x => x.Event.OnEnterInteractableStageGimmickSubject().OnNext(this))
                .AddTo(this);

            this.OnTriggerExit2DAsObservable()
                .Select(x => x.attachedRigidbody.GetComponent<Actor>())
                .Where(x => x != null)
                .Subscribe(x => x.Event.OnExitInteractableStageGimmickSubject().OnNext(this))
                .AddTo(this);
        }
    }
}
