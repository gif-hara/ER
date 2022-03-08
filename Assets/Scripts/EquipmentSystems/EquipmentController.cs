using ER.ERBehaviour;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.EquipmentSystems
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EquipmentController : MonoBehaviour, IDisposable, IBehaviourData
    {
        [SerializeField]
        private EquipmentData data;

        private CompositeDisposable activeDisposable = new CompositeDisposable();

        public EquipmentController Attach(IActor actor)
        {
            var clone = Instantiate(this, actor.transform);
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            var behaviourData = new EquipmentBehaviourData
            {
                Actor = actor
            };

            foreach(var behaviour in clone.data.behaviours)
            {
                behaviour.AsObservable(behaviourData)
                    .Subscribe()
                    .AddTo(this.activeDisposable);
            }

            return clone;
        }

        public void Dispose()
        {
            Destroy(this.gameObject);
            this.activeDisposable.Dispose();
        }
    }
}
