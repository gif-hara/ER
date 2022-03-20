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
        private bool evalute = false;

        private IDisposable disposable = null;

        public bool Evalute(IBehaviourData data)
        {
            if (this.disposable == null)
            {
                var actor = data.Cast<IActorHolder>().Actor;
                disposable = actor.Event.OnBeginLeftEquipmentSubject()
                    .Subscribe(_ => this.evalute = true);
            }

            var result = this.evalute;

            if (result)
            {
                this.evalute = false;
            }

            return result;
        }
    }
}
