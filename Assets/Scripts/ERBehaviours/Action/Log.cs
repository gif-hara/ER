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

        public void Invoke(IBehaviourData data)
        {
            Debug.Log(this.message);
        }
    }
}
