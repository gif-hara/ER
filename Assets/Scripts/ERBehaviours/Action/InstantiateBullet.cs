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

        [SerializeField]
        private int power = default;

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var actorHolder = data.Cast<IActorHolder>();
                var equipmentControllerHolder = data.Cast<IEquipmentControllerHolder>();

                this.bulletPrefab.Instantiate(
                    actorHolder.Actor,
                    equipmentControllerHolder.EquipmentController,
                    this.power
                    );

                return Observable.ReturnUnit();
            });
        }
    }
}
