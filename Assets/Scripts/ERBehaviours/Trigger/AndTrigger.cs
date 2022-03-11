using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 複数の<see cref="ITrigger"/>の条件を満たした場合に発火できる<see cref="ITrigger"/>
    /// </summary>
    [Serializable]
    public sealed class AndTrigger : ITrigger
    {
        [SerializeReference, SubclassSelector(typeof(ITrigger))]
        private List<ITrigger> triggers = default;

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                return this.triggers
                .Select(x => x.AsObservable(data))
                .ZipLatest()
                .AsUnitObservable();
            });
        }
    }
}
