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

        private bool evalute = false;

        private IDisposable disposable = null;

        public bool Evalute(IBehaviourData data)
        {
            if (this.disposable == null)
            {
                this.disposable = Observable.Timer(TimeSpan.FromSeconds(this.delaySeconds))
                    .Subscribe(_ => this.evalute = true);
            }

            return this.evalute;
        }
    }
}
