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
    public sealed class EquipmentController : MonoBehaviour, IBehaviourData
    {
        [SerializeField]
        private PlayableDirector playableDirector = default;

        [SerializeField]
        private PlayableAsset defaultPlayableAsset = default;

        [SerializeField]
        public List<ERBehaviour.Behaviour> behaviours = default;

        /// <summary>
        /// 攻撃した際のダメージ係数
        /// </summary>
        public float Power { get; set; }

        public IEquipmentData EquipmentData { get; private set; }

        public Actor Actor { get; private set; }

        public PlayableDirector PlayableDirector => this.playableDirector;

        private CompositeDisposable disposables = new CompositeDisposable();

        private List<Collider2D> colliders = null;

        public EquipmentController Attach(Actor actor, IEquipmentData equipmentData)
        {
            var clone = Instantiate(this, actor.transform);
            clone.AttachInternal(actor, equipmentData);
            return clone;
        }

        void OnDestroy()
        {
            this.disposables.Dispose();
        }

        private void AttachInternal(Actor actor, IEquipmentData equipmentData)
        {
            this.Actor = actor;
            this.EquipmentData = equipmentData;
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
                    hitActor.Event.OnHitOpponentAttackSubject().OnNext(this);
                }
            })
            .AddTo(this.disposables);

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    foreach (var i in this.behaviours)
                    {
                        i.Invoke(behaviourData, this.disposables);
                    }
                });

            this.colliders = new List<Collider2D>(this.GetComponentsInChildren<Collider2D>());

            this.PlayDefaultPlayableAsset();

            actor.Event.OnChangedStateSubject()
                .Subscribe(x =>
                {
                    if (x == ActorStateController.StateType.Movable)
                    {
                        this.PlayDefaultPlayableAsset();
                    }
                })
                .AddTo(this.disposables);
        }

        public void PlayDefaultPlayableAsset()
        {
            this.playableDirector.extrapolationMode = DirectorWrapMode.Loop;
            this.playableDirector.Play(this.defaultPlayableAsset);

            foreach (var i in this.colliders)
            {
                i.gameObject.SetActive(false);
            }
        }

        private static int GetLayerIndex(int ownerLayerIndex)
        {
            switch (ownerLayerIndex)
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
