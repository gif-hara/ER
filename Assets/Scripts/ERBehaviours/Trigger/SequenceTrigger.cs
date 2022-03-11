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
    public sealed class SequenceTrigger : ITrigger
    {
        [SerializeReference, SubclassSelector(typeof(ITrigger))]
        private List<ITrigger> triggers = default;

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var stream = this.triggers[0].AsObservable(data);
                for(var i=1; i<this.triggers.Count; i++)
                {
                    var index = i;
                    stream = stream.SelectMany(_ => this.triggers[index].AsObservable(data));
                }

                return stream;
            });
        }
    }
}
