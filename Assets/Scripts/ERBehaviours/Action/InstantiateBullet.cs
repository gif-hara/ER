using System;
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

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var actorHolder = data as IActorHolder;
                Assert.IsNotNull(actorHolder);

                this.bulletPrefab.Instantiate(actorHolder.Actor);

                return Observable.ReturnUnit();
            });
        }
    }
}
