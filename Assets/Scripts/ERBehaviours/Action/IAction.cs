using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// アクションを実行して完了までのイベント
        /// </summary>
        IObservable<Unit> AsObservable(IBehaviourData data);
    }
}
