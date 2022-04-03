using ER.EquipmentSystems;
using ER.MasterDataSystem;
using HK.Framework.EventSystems;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Actor : MonoBehaviour, IActor
    {
        public string Id { get; private set; }

        [SerializeField]
        private ActorMotionData motionData = default;

        [SerializeField]
        private ActorAnimationMediator animationParameter = default;

        [SerializeField]
        private PlayableDirector director = default;

        [SerializeField]
        private ActorRecoveryItemController recoveryItemController = default;

        private MessageBroker broker = new MessageBroker();

        public ActorStatusController StatusController { get; } = new ActorStatusController();

        public ActorInteractableStageGimmickController InteractableStageGimmickController { get; } = new ActorInteractableStageGimmickController();

        public ActorEquipmentController EquipmentController { get; } = new ActorEquipmentController();

        public ActorInventoryController InventoryController { get; } = new ActorInventoryController();

        public ActorStateController StateController { get; } = new ActorStateController();

        public ActorDirectorController DirectorController { get; } = new ActorDirectorController();

        public Animator Animator { get; private set; }

        public ActorAnimationMediator AnimationParameter => this.animationParameter;

        public ActorMotionController MotionController { get; private set; }

        public CompositeDisposable Disposables { get; } = new CompositeDisposable();

        public IMessageBroker Broker => this.broker;

        public Rigidbody2D Rigidbody2D { get; private set; }

        /// <summary>
        /// シーンに存在する<see cref="Actor"/>
        /// </summary>
        public static readonly List<Actor> Actors = new List<Actor>();

        /// <summary>
        /// シーンに存在するプレイヤーリスト
        /// </summary>
        public static readonly List<Actor> Players = new List<Actor>();

        /// <summary>
        /// シーンに存在する敵リスト
        /// </summary>
        public static readonly List<Actor> Enemies = new List<Actor>();

        public Actor Spawn(Vector3 position, Quaternion rotation, string actorId)
        {
            var clone = Instantiate(this, position, rotation);
            clone.Setup(actorId);

            return clone;
        }

        private void Setup(string actorId)
        {
            this.Id = actorId;
            this.Animator = this.GetComponent<Animator>();
            this.StateController.Setup(this);
            this.StatusController.Setup(this, MasterDataActorStatus.Get(actorId).statusData);
            this.InteractableStageGimmickController.Setup(this);
            this.EquipmentController.Setup(this);
            this.MotionController = new ActorMotionController();
            this.MotionController.Setup(this, this.motionData);
            this.DirectorController.Setup(this, this.director);
            this.recoveryItemController.Setup(this);
            this.InventoryController.Setup(this);

            this.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    this.Disposables.Dispose();
                    this.broker.Dispose();
                });
        }

        void Awake()
        {
            this.Rigidbody2D = this.GetComponent<Rigidbody2D>();
            Assert.IsNotNull(this.Rigidbody2D);
        }

        void Start()
        {
            Actors.Add(this);

            if (this.gameObject.layer == Layer.Index.Player)
            {
                Players.Add(this);
            }
            else if (this.gameObject.layer == Layer.Index.Enemy)
            {
                Enemies.Add(this);
            }
            else
            {
                Debug.LogWarning($"{this.name}.{nameof(this.gameObject.layer)}はどの{typeof(Actor)}にも属していません");
            }

            GameController.Instance.Broker.Publish(GameEvent.OnSpawnedActor.Get(this));
        }

        void OnDestroy()
        {
            Actors.Remove(this);

            if (this.gameObject.layer == Layer.Index.Player)
            {
                Players.Remove(this);
            }
            else if (this.gameObject.layer == Layer.Index.Enemy)
            {
                Enemies.Remove(this);
            }
            else
            {
                Debug.LogWarning($"{this.name}.{nameof(this.gameObject.layer)}はどの{typeof(Actor)}にも属していません");
            }
        }

        void OnAnimatorMove()
        {
            var velocity = this.Animator.rootPosition - this.transform.position;
            this.MotionController.MoveRaw(velocity);
        }
    }
}
