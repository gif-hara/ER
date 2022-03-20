using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class Timer : ITrigger
    {
        [SerializeField]
        private float delaySeconds = default;

        private bool evaluate = false;

        private IDisposable disposable = null;

        public bool Evaluate(IBehaviourData data)
        {
            if (this.disposable == null)
            {
                this.disposable = Observable.Timer(TimeSpan.FromSeconds(this.delaySeconds))
                    .Subscribe(_ => this.evaluate = true);
            }

            return this.evaluate;
        }
    }
}
