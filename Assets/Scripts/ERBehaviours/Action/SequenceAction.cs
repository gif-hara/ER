using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class SequenceAction : IAction
    {
        [SerializeReference, SubclassSelector(typeof(IAction))]
        private List<IAction> actions = default;

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var stream = this.actions[0].AsObservable(data);
                for(var i=1; i<this.actions.Count; i++)
                {
                    var index = i;
                    stream = stream.SelectMany(_ => this.actions[index].AsObservable(data));
                }

                return stream;
            });
        }
    }
}
