using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Log : IAction
    {
        [SerializeField]
        private string message;

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                Debug.Log(this.message);
                return Observable.ReturnUnit();
            });
        }
    }
}
