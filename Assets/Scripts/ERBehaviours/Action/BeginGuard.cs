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

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var behaviourData = data.Cast<IActorHolder>();
                var equipmentController = behaviourData.Actor.EquipmentController.GetEquipmentController(this.handType);
                var director = equipmentController.PlayableDirector;
                var actor = behaviourData.Actor;

                director.extrapolationMode = this.wrapMode;
                director.playableAsset = this.playableAsset;
                director.SetGenericBinding("ActorAnimation", actor.Animator);
                director.Play();

                actor.StateController.ChangeRequest(ActorStateController.StateType.Guard);

                actor.Event.OnEndLeftEquipmentSubject()
                .Take(1)
                .TakeUntil(actor.Event.OnChangedStateSubject().Where(x => x == ActorStateController.StateType.Attack))
                .Subscribe(_ =>
                {
                    actor.StateController.ChangeRequest(ActorStateController.StateType.Movable);
                })
                .AddTo(equipmentController)
                .AddTo(actor.Disposables);

                actor.Event.OnChangedStateSubject()
                .Where(x => x == ActorStateController.StateType.Attack)
                .Take(1)
                .Subscribe(_ =>
                {
                    equipmentController.PlayDefaultPlayableAsset();
                })
                .AddTo(equipmentController)
                .AddTo(actor.Disposables);

                return Observable.ReturnUnit();
            });
        }
    }
}
