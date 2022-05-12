using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class TimerRandom : ITrigger
    {
        [SerializeField]
        private float minSeconds = default;

        [SerializeField]
        private float maxSeconds = default;

        /// <summary>
        /// 指定した時間になったら一度のみ発火するか
        /// <c>false</c>の場合は指定した時間を超えたら常に発火します
        /// </summary>
        [SerializeField]
        private bool oneShot;

        private bool evaluate = false;

        private IDisposable disposable = null;

        public bool Evaluate(IBehaviourData data)
        {
            if (this.disposable == null)
            {
                this.evaluate = false;
                this.disposable = Observable.Timer(TimeSpan.FromSeconds(Random.Range(this.minSeconds, this.maxSeconds)))
                    .Subscribe(_ =>
                    {
                        this.evaluate = true;
                    });
            }

            if (this.evaluate)
            {
                if (this.oneShot)
                {
                    this.disposable?.Dispose();
                    this.disposable = null;
                }
            }

            return this.evaluate;
        }
    }
}
