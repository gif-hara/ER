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
            Avoidance,
        }

        private StateController<StateType> stateController = new StateController<StateType>(StateType.Invalid);

        public StateType CurrentState => this.stateController.CurrentState;

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

            actor.Event.OnRespawnedSubject()
                .Subscribe(_ =>
                {
                    this.ChangeRequest(StateType.Movable);
                })
                .AddTo(disposables);

            this.ChangeRequest(StateType.Movable);
        }

        public void ChangeRequest(StateType stateType)
        {
            this.stateController.ChangeRequest(stateType);
        }
    }
}
