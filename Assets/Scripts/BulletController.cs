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

        private CompositeDisposable disposable = new CompositeDisposable();

        public BulletController Instantiate(IActor actor, EquipmentController equipmentController)
        {
            var clone = Instantiate(this, equipmentController.transform);
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            clone.gameObject.SetLayerRecursive(equipmentController.gameObject.layer);

            clone.OnCollisionEnter2DAsObservable()
                .Subscribe(x =>
                {
                    Debug.Log(x);
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
