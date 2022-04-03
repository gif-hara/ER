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

        [SerializeField]
        private PoolableEffect hitEffectPrefab = default;

        /// <summary>
        /// 攻撃した際のダメージ係数
        /// </summary>
        public float Power { get; set; }

        public string ItemInstanceId { get; private set; }

        /// <summary>
        /// この装備品と紐づく<see cref="Item"/>を返す
        /// </summary>
        public Item Item => this.Actor.InventoryController.Equipments[this.ItemInstanceId];

        public Actor Actor { get; private set; }

        public PlayableDirector PlayableDirector => this.playableDirector;

        private CompositeDisposable disposables = new CompositeDisposable();

        private List<Collider2D> colliders = null;

        public EquipmentController Attach(Actor actor, string itemInstanceId)
        {
            var clone = Instantiate(this, actor.transform);
            clone.AttachInternal(actor, itemInstanceId);
            return clone;
        }

        void OnDestroy()
        {
            this.disposables.Dispose();
        }

        private void AttachInternal(Actor actor, string itemInstanceId)
        {
            this.Actor = actor;
            this.ItemInstanceId = itemInstanceId;
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            this.gameObject.SetLayerRecursive(GetLayerIndex(actor.gameObject.layer));
            var behaviourData = new EquipmentBehaviourData
            {
                Actor = actor,
                EquipmentController = this,
            };

            this.OnTriggerEnter2DAsObservable()
            .Subscribe(x =>
            {
                var hitActor = x.attachedRigidbody.GetComponent<IActor>();
                if (hitActor != null)
                {
                    hitActor.Broker.Publish(ActorEvent.OnHitOpponentAttack.Get(this));

                    if(!hitActor.AnimationParameter.invisible && this.hitEffectPrefab != null)
                    {
                        this.hitEffectPrefab.Rent(x.bounds.center, Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f)));
                    }
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

            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                .Subscribe(x =>
                {
                    if (x.NextState == ActorStateController.StateType.Movable)
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
