using ER.ActorControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.StageControllers
{
    /// <summary>
    /// チェックポイントを更新する<see cref="InteractableStageGimmick"/>
    /// </summary>
    public sealed class InteractableStageGimmickCheckPoint : InteractableStageGimmick
    {
        public override void BeginInteract(Actor actor)
        {
            actor.Broker.Publish(ActorEvent.OnInteractedCheckPoint.Get(this));
        }
    }
}
