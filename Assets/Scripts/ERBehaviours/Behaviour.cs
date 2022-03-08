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

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return this.trigger.AsObservable(data)
                .SelectMany(_ => this.action.AsObservable(data));
        }
    }
}
