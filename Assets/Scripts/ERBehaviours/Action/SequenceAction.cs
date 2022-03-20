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

        public void Invoke(IBehaviourData data)
        {
            foreach (var i in this.actions)
            {
                i.Invoke(data);
            }
        }
    }
}
