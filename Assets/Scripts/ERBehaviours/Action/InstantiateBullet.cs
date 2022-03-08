using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InstantiateBullet : IAction
    {
        [SerializeField]
        private BulletController bulletPrefab = default;

        [SerializeField]
        private int power = default;

        [SerializeReference, SubclassSelector(typeof(IAction))]
        private List<IAction> onInstantiateActions = default;

        [SerializeReference, SubclassSelector(typeof(IAction))]
        private List<IAction> onCompleteActions = default;

        private CompositeDisposable disposables = new CompositeDisposable();

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var actorHolder = data.Cast<IActorHolder>();
                var equipmentControllerHolder = data.Cast<IEquipmentControllerHolder>();

                var bullet = this.bulletPrefab.Instantiate(
                    actorHolder.Actor,
                    equipmentControllerHolder.EquipmentController,
                    this.power
                    );

                foreach(var i in this.onInstantiateActions)
                {
                    i.AsObservable(null)
                    .Subscribe()
                    .AddTo(this.disposables);
                }

                bullet.OnCompleteAsObservable()
                .Subscribe(_ =>
                {
                    foreach (var i in this.onCompleteActions)
                    {
                        i.AsObservable(null)
                        .Subscribe()
                        .AddTo(this.disposables);
                    }
                })
                .AddTo(this.disposables);

                return Observable.ReturnUnit();
            });
        }
    }
}
