using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrigger
    {
        bool Evalute(IBehaviourData data);
    }
}
