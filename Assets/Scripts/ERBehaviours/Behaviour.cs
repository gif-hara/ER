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
    public sealed class Behaviour
    {
        [SerializeReference, SubclassSelector(typeof(ITrigger))]
        private ITrigger trigger;

        [SerializeReference, SubclassSelector(typeof(IAction))]
        private IAction action;

        public void Invoke(IBehaviourData data, CompositeDisposable disposables)
        {
            if (!this.trigger.Evaluate(data))
            {
                return;
            }

            this.action.Invoke(data);
        }
    }
}
