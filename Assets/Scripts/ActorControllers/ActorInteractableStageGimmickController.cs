using UnityEngine;
using UnityEngine.Assertions;
using ER.StageControllers;
using UniRx;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>と<see cref="IInteractableStageGimmick"/>の制御を行うクラス
    /// </summary>
    public sealed class ActorInteractableStageGimmickController
    {
        private Actor actor;

        private IInteractableStageGimmick currentGimmick = null;

        public void Setup(Actor actor)
        {
            this.actor = actor;

            actor.Broker.Receive<ActorEvent.OnEnterInteractableStageGimmick>()
                .Subscribe(x => this.currentGimmick = x.Gimmick)
                .AddTo(actor.Disposables);

            actor.Broker.Receive<ActorEvent.OnExitInteractableStageGimmick>()
                .Where(x => this.currentGimmick == x.Gimmick)
                .Subscribe(x => this.currentGimmick = null)
                .AddTo(actor.Disposables);
        }

        public void BeginInteract()
        {
            if (this.currentGimmick == null)
            {
                return;
            }
            this.currentGimmick.BeginInteract(this.actor);
        }
    }
}
