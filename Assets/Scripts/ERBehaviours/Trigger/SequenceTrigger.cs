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

        public bool Evaluate(IBehaviourData data)
        {
            throw new NotImplementedException();
        }
    }
}
