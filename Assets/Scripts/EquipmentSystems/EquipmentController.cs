using ER.ActorControllers;
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
            clone.gameObject.SetLayerRecursive(GetLayerIndex(actor.gameObject.layer));
            var behaviourData = new EquipmentBehaviourData
            {
                Actor = actor,
                EquipmentController = clone,
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

        private static int GetLayerIndex(int ownerLayerIndex)
        {
            switch(ownerLayerIndex)
            {
                case Layer.Index.Player:
                    return Layer.Index.PlayerBullet;
                case Layer.Index.Enemy:
                    return Layer.Index.EnemyBullet;
                default:
                    Assert.IsTrue(false, $"{ownerLayerIndex}は未対応です");
                    return 0;
            }
        }
    }
}
