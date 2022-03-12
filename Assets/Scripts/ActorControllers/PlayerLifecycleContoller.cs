using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using System;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PlayerLifecycleContoller : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;

        private Vector3 checkPoint;

        private void Start()
        {
            this.actor.Event.OnDeadSubject()
                .SelectMany(_ => this.PlayDeadEffectAsync())
                .Subscribe()
                .AddTo(this);

            this.checkPoint = this.actor.transform.position;
        }

        private IObservable<Unit> PlayDeadEffectAsync()
        {
            return Observable.Defer(() =>
            {
                this.actor.gameObject.SetActive(false);

                return Observable
                .Timer(TimeSpan.FromSeconds(2.0f))
                .Do(_ =>
                {
                    this.actor.transform.position = this.checkPoint;
                    this.actor.gameObject.SetActive(true);
                    this.actor.Event.OnRespawnedSubject().OnNext(Unit.Default);
                })
                .AsUnitObservable();
            });
        }
    }
}
