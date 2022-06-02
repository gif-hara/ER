using System;
using UniRx;
using UnityEngine;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PublishOnRequestOpenThankYouForPlaying : IAction
    {
        [SerializeField]
        private float delaySeconds;
        
        public void Invoke(IBehaviourData data)
        {
            Observable.Timer(TimeSpan.FromSeconds(this.delaySeconds))
                .Subscribe(_ =>
                {
                    GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenThankYouForPlaying.Get());
                });
        }
    }
}
