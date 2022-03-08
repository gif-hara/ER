using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Destroy : IAction
    {
        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var gameObjectHolder = data.Cast<IGameObjectHolder>();

                UnityEngine.Object.Destroy(gameObjectHolder.GameObject);

                return Observable.ReturnUnit();
            });
        }
    }
}
