using ER.ActorControllers;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 防御を開始する
    /// </summary>
    public sealed class BeginGuard : IAction
    {
        [SerializeField]
        private PlayableAsset playableAsset = default;

        [SerializeField]
        private DirectorWrapMode wrapMode = default;

        [SerializeField]
        private HandType handType = default;

        public void Invoke(IBehaviourData data)
        {
            var behaviourData = data.Cast<IActorHolder>();
            var equipmentController = behaviourData.Actor.EquipmentController.GetEquipmentController(this.handType);
            var director = equipmentController.PlayableDirector;
            var actor = behaviourData.Actor;

            director.extrapolationMode = this.wrapMode;
            director.playableAsset = this.playableAsset;
            director.SetGenericBinding("ActorAnimation", actor.Animator);
            director.Play();

            actor.EquipmentController.BeginGuard(equipmentController);
            actor.StateController.ChangeRequest(ActorStateController.StateType.Guard);

            actor.Broker.Receive<ActorEvent.EndEquipment>()
                .Where(x => x.HandType == HandType.Left)
                .Take(1)
                .TakeUntil(actor.Broker.Receive<ActorEvent.OnChangedStateType>().Where(x => x.NextState == ActorStateController.StateType.Attack || x.NextState == ActorStateController.StateType.Movable))
                .Subscribe(_ =>
                {
                    actor.StateController.ChangeRequest(ActorStateController.StateType.Movable);
                })
                .AddTo(equipmentController)
                .AddTo(actor.Disposables);

            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                .Where(x => x.NextState == ActorStateController.StateType.Attack || x.NextState == ActorStateController.StateType.Movable)
                .Take(1)
                .Subscribe(_ =>
                {
                    equipmentController.PlayDefaultPlayableAsset();
                    actor.EquipmentController.EndGuard();
                })
                .AddTo(equipmentController)
                .AddTo(actor.Disposables);
        }
    }
}
