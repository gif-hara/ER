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
        public void Invoke(IBehaviourData data)
        {
            var gameObjectHolder = data.Cast<IGameObjectHolder>();

            UnityEngine.Object.Destroy(gameObjectHolder.GameObject);
        }
    }
}
