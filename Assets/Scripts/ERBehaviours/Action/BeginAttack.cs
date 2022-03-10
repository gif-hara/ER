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
                var playableDirector = behaviourData.EquipmentController.PlayableDirector;

                playableDirector.extrapolationMode = this.wrapMode;
                playableDirector.playableAsset = this.playableAsset;
                var binding = playableDirector.playableAsset.outputs.First(c => c.streamName == "ActorAnimation");

                playableDirector.Play();
                playableDirector.SetGenericBinding(binding.sourceObject, behaviourData.Actor.Animator);

                behaviourData.EquipmentController.Power = this.power;


                return Observable.ReturnUnit();
            });
        }
    }
}
