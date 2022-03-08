using ER.EquipmentSystems;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx.Triggers;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BulletController : MonoBehaviour
    {
        [SerializeField]
        private List<ERBehaviour.Behaviour> behaviours = default;

        public int Power { get; private set; }

        private CompositeDisposable disposable = new CompositeDisposable();

        public BulletController Instantiate(
            IActor actor,
            EquipmentController equipmentController,
            int power
            )
        {
            var clone = Instantiate(this, equipmentController.transform);
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            clone.gameObject.SetLayerRecursive(equipmentController.gameObject.layer);

            clone.Power = power;

            clone.OnCollisionEnter2DAsObservable()
                .Subscribe(x =>
                {
                    var hitActor = x.rigidbody.GetComponent<IActor>();
                    if (hitActor != null)
                    {
                        hitActor.OnCollisionBullet(this);
                    }
                })
                .AddTo(disposable);

            var behaviourData = new BulletBehaviourData
            {
                GameObject = clone.gameObject
            };

            foreach(var behaviour in this.behaviours)
            {
                behaviour.AsObservable(behaviourData)
                    .Subscribe()
                    .AddTo(this.disposable);
            }

            return clone;
        }

        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
