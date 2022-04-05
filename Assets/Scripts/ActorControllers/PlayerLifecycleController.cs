using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using System;
using ER.UIPresenters;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PlayerLifecycleController : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;

        [SerializeField]
        private PoolableEffect deadEffectPrefab = default;

        private void Start()
        {
            this.actor.Broker.Receive<ActorEvent.OnDead>()
                .SelectMany(_ => this.PlayDeadEffectAsync())
                .Subscribe()
                .AddTo(this);
        }

        private IObservable<Unit> PlayDeadEffectAsync()
        {
            return Observable.Defer(() =>
            {
                this.actor.gameObject.SetActive(false);
                var t = this.actor.transform;
                this.deadEffectPrefab.Rent(t.position, t.rotation);

                return Observable
                .Timer(TimeSpan.FromSeconds(2.0f))
                .SelectMany(_ => FadePresenter.Instance.PlayOutAsync())
                .SelectMany(_ =>
                {
                    this.actor.transform.position = this.actor.MotionController.CheckPoint;
                    this.actor.gameObject.SetActive(true);
                    this.actor.Broker.Publish(ActorEvent.OnRespawned.Get());
                    return Observable.ReturnUnit();
                })
                .SelectMany(_ => FadePresenter.Instance.PlayInAsync())
                .AsUnitObservable();
            });
        }
    }
}
