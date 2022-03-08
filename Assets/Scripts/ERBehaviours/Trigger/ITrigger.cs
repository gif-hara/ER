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
        /// <summary>
        /// トリガーが発火された際のイベント
        /// </summary>
        IObservable<Unit> AsObservable(IBehaviourData data);
    }
}
