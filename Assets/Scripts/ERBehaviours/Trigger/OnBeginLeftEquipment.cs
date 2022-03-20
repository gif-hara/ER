using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class OnBeginLeftEquipment : ITrigger
    {
        private bool evaluate = false;

        private IDisposable disposable = null;

        public bool Evaluate(IBehaviourData data)
        {
            if (this.disposable == null)
            {
                var actor = data.Cast<IActorHolder>().Actor;
                disposable = actor.Event.OnBeginLeftEquipmentSubject()
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
