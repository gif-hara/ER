using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using UniRx.Triggers;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorStateController
    {
        public enum StateType
        {
            Invalid,
            Movable,
            Attack,
        }

        private StateController<StateType> stateController = new StateController<StateType>(StateType.Invalid);

        public void Setup(IActor actor, CompositeDisposable disposables)
        {
            this.stateController.OnChangedStateAsObservable()
                .Subscribe(x =>
                {
                    actor.Event.OnChangedStateSubject().OnNext(x);
                })
                .AddTo(disposables);

            actor.gameObject.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    this.stateController.Update();
                })
                .AddTo(disposables);
        }

        public void ChangeRequest(StateType stateType)
        {
            this.stateController.ChangeRequest(stateType);
        }
    }
}
