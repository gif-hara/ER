using ER.ActorControllers;
using ER.ERBehaviour;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER.EquipmentSystems
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EquipmentController : MonoBehaviour, IDisposable, IBehaviourData
    {
        [SerializeField]
        private PlayableDirector playableDirector = default;

        [SerializeField]
        private PlayableAsset defaultPlayableAsset = default;

        [SerializeField]
        public List<ERBehaviour.Behaviour> behaviours = default;

        public PlayableDirector PlayableDirector => this.playableDirector;

        private CompositeDisposable activeDisposable = new CompositeDisposable();

        public EquipmentController Attach(IActor actor)
        {
            var clone = Instantiate(this, actor.transform);
            clone.AttachInternal(actor);
            return clone;
        }

        public void Dispose()
        {
            Destroy(this.gameObject);
            this.activeDisposable.Dispose();
        }

        private void AttachInternal(IActor actor)
        {
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            this.gameObject.SetLayerRecursive(GetLayerIndex(actor.gameObject.layer));
            var behaviourData = new EquipmentBehaviourData
            {
                Actor = actor,
                EquipmentController = this,
            };

            this.OnCollisionEnter2DAsObservable()
            .Subscribe(x =>
            {
                var hitActor = x.rigidbody.GetComponent<IActor>();
                if (hitActor != null)
                {
                    hitActor.OnCollisionOpponentAttack(this);
                }
            })
            .AddTo(this.activeDisposable);


            foreach (var behaviour in this.behaviours)
            {
                behaviour.AsObservable(behaviourData)
                    .Subscribe()
                    .AddTo(this.activeDisposable);
            }

            this.PlayDefaultPlayableAsset();
        }

        private void PlayDefaultPlayableAsset()
        {
            this.playableDirector.extrapolationMode = DirectorWrapMode.Loop;
            this.playableDirector.Play(this.defaultPlayableAsset);
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
