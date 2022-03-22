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
            Guard,
        }

        private StateController<StateType> stateController = new StateController<StateType>(StateType.Invalid);

        public StateType CurrentState => this.stateController.CurrentState;

        public void Setup(IActor actor)
        {
            this.stateController.OnChangedStateAsObservable()
                .Subscribe(x =>
                {
                    actor.Broker.Publish(ActorEvent.OnChangedStateType.Get(x));
                })
                .AddTo(actor.Disposables);

            actor.gameObject.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    this.stateController.Update();
                })
                .AddTo(actor.Disposables);

            actor.Event.OnRespawnedSubject()
                .Subscribe(_ =>
                {
                    this.ChangeRequest(StateType.Movable);
                })
                .AddTo(actor.Disposables);

            this.ChangeRequest(StateType.Movable);
        }

        public void ChangeRequest(StateType stateType)
        {
            this.stateController.ChangeRequest(stateType);
        }
    }
}
