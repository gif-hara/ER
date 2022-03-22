using System;
using ER.ActorControllers;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class OnBeginEquipment : ITrigger
    {
        [SerializeField]
        private HandType handType = default;

        private bool evaluate = false;

        private IDisposable disposable = null;

        public bool Evaluate(IBehaviourData data)
        {
            if (this.disposable == null)
            {
                var actor = data.Cast<IActorHolder>().Actor;
                this.disposable = actor.Broker.Receive<ActorEvent.BeginEquipment>()
                    .Where(x => x.HandType == this.handType)
                    .Subscribe(_ => this.evaluate = true);
            }

            var result = this.evaluate;

            if (result)
            {
                this.evaluate = false;
            }

            return result;
        }
    }
}
