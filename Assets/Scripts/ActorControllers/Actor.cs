using ER.EquipmentSystems;
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
        [SerializeField]
        private ActorMotionData motionData = default;

        [SerializeField]
        private ActorAnimationParameter animationParameter = default;

        [SerializeField]
        private PlayableDirector director = default;

        private MessageBroker broker = new MessageBroker();

        public ActorStatusController StatusController { get; } = new ActorStatusController();

        public ActorInteractableStageGimmickController InteractableStageGimmickController { get; } = new ActorInteractableStageGimmickController();

        public ActorEquipmentController EquipmentController { get; } = new ActorEquipmentController();

        public ActorInventoryController InventoryController { get; } = new ActorInventoryController();

        public ActorStateController StateController { get; } = new ActorStateController();

        public ActorDirectorController DirectorController { get; } = new ActorDirectorController();

        public Animator Animator { get; private set; }

        public ActorEvent Event { get; private set; }

        public ActorAnimationParameter AnimationParameter => this.animationParameter;

        public ActorMotionController MotionController { get; private set; }

        public CompositeDisposable Disposables { get; } = new CompositeDisposable();

        public IMessageBroker Broker => this.broker;

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

        public Actor Spawn(Vector3 position, Quaternion rotation, ActorStatusData statusData)
        {
            var clone = Instantiate(this, position, rotation);
            clone.Setup(statusData);

            return clone;
        }

        private void Setup(ActorStatusData statusData)
        {
            this.Animator = this.GetComponent<Animator>();
            this.Event = new ActorEvent();
            this.StateController.Setup(this);
            this.StatusController.Setup(this, statusData);
            this.InteractableStageGimmickController.Setup(this);
            this.EquipmentController.Setup(this);
            this.MotionController = new ActorMotionController();
            this.MotionController.Setup(this, this.motionData);
            this.DirectorController.Setup(this, this.director);

            this.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    this.Disposables.Dispose();
                    this.Event.Dispose();
                    this.broker.Dispose();
                });
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

            GameController.Instance.Event.OnSpawnedActorSubject().OnNext(this);
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
