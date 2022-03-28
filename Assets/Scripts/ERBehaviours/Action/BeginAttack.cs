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
    /// 攻撃を開始する
    /// </summary>
    public sealed class BeginAttack : IAction
    {
        [SerializeField]
        private PlayableAsset playableAsset = default;

        [SerializeField]
        private DirectorWrapMode wrapMode = DirectorWrapMode.None;

        [SerializeField]
        private float power = 1.0f;

        [SerializeField]
        private HandType handType = HandType.Right;

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

            equipmentController.Power = this.power;

            actor.StateController.ChangeRequest(ActorStateController.StateType.Attack);

            director.OnStoppedAsObservable()
            .Subscribe(_ =>
            {
                actor.StateController.ChangeRequest(ActorStateController.StateType.Movable);
            })
            .AddTo(equipmentController);

            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
            .Where(x => x.NextState == ActorStateController.StateType.Avoidance || x.NextState == ActorStateController.StateType.Guard)
            .Subscribe(_ =>
            {
                equipmentController.PlayDefaultPlayableAsset();
            })
            .AddTo(equipmentController);
        }
    }
}
