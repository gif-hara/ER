using ER.EquipmentSystems;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

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
            this.disposable.Dispose();
        }
    }
}
