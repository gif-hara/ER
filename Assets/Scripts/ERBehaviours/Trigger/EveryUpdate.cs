using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class EveryUpdate : ITrigger
    {
        public bool Evaluate(IBehaviourData data)
        {
            return true;
        }
    }
}
