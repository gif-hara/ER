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
        public List<ERBehaviour.Behaviour> behaviours = default;

        [SerializeField]
        private GameObject behaviourHolderParent = default;

        [SerializeField]
        private PoolableEffect hitEffectPrefab = default;

        /// <summary>
        /// 当たり判定を持つオブジェクト
        /// </summary>
        [SerializeField]
        private GameObject colliderObject = default;

        /// <summary>
        /// 攻撃した際のダメージ係数
        /// </summary>
        public float Power { get; set; }
        
        /// <summary>
        /// ノックバック蓄積値の係数
        /// </summary>
        public float KnockBackAccumulate { get; set; }
        
        public string ItemInstanceId { get; private set; }

        /// <summary>
        /// この装備品と紐づく<see cref="Item"/>を返す
        /// </summary>
        public Item Item => this.Actor.InventoryController.Equipments[this.ItemInstanceId];

        public Actor Actor { get; private set; }

        private CompositeDisposable disposables = new CompositeDisposable();

        private BehaviourHolder[] behaviourHolders;

        public EquipmentController Attach(Actor actor, string itemInstanceId, HandType handType)
        {
            var clone = Instantiate(this, actor.BodyController.GetHandParent(handType));
            clone.AttachInternal(actor, itemInstanceId);
            return clone;
        }

        void OnDestroy()
        {
            this.disposables.Dispose();
        }

        public void SetupCollider(bool isActive, float power, float knockBackAccumulate)
        {
            if (this.colliderObject == null)
            {
                return;
            }
            
            this.colliderObject.SetActive(isActive);
            this.Power = power;
            this.KnockBackAccumulate = knockBackAccumulate;
        }

        private void AttachInternal(Actor actor, string itemInstanceId)
        {
            this.Actor = actor;
            this.ItemInstanceId = itemInstanceId;
            this.behaviourHolders = this.behaviourHolderParent.GetComponentsInChildren<BehaviourHolder>();
            var t = this.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
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
            .AddTo(this);

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    foreach (var i in this.behaviourHolders)
                    {
                        i.Invoke(behaviourData, this.disposables);
                    }
                });

            actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                .Subscribe(x =>
                {
                    if (x.NextState == ActorStateController.StateType.Movable)
                    {
                        this.SetupCollider(false, 0.0f, 0.0f);
                    }
                })
                .AddTo(this);
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
