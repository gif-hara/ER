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
        private DirectorWrapMode wrapMode = default;

        [SerializeField]
        private int power = default;

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var behaviourData = data.Cast<EquipmentBehaviourData>();
                var director = behaviourData.EquipmentController.PlayableDirector;

                director.extrapolationMode = this.wrapMode;
                director.playableAsset = this.playableAsset;
                director.SetGenericBinding("ActorAnimation", behaviourData.Actor.Animator);
                director.Play();

                behaviourData.EquipmentController.Power = this.power;

                behaviourData.Actor.StateController.ChangeRequest(ActorStateController.StateType.Attack);

                Observable.FromEvent<PlayableDirector>(x => director.stopped += x, x => director.stopped -= x)
                .Take(1)
                .Subscribe(_ =>
                {
                    behaviourData.Actor.StateController.ChangeRequest(ActorStateController.StateType.Movable);
                });

                return Observable.ReturnUnit();
            });
        }
    }
}
