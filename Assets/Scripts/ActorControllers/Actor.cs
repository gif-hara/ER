using ER.EquipmentSystems;
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
        private ActorStatusData statusData = default;

        [SerializeField]
        private ActorMotionData motionData = default;

        [SerializeField]
        private ActorAnimationParameter animationParameter = default;

        [SerializeField]
        private PlayableDirector director = default;

        public ActorStatusController StatusController { get; } = new ActorStatusController();

        public ActorStateController StateController { get; } = new ActorStateController();

        public ActorDirectorController DirectorController { get; } = new ActorDirectorController();

        private EquipmentController rightEquipment;

        public Animator Animator { get; private set; }

        public ActorEvent Event { get; private set; }

        public ActorAnimationParameter AnimationParameter => this.animationParameter;

        public ActorMotionController MotionController { get; private set; }

        public CompositeDisposable Disposables { get; } = new CompositeDisposable();

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

        void Awake()
        {
            this.Animator = this.GetComponent<Animator>();
            this.Event = new ActorEvent();
            this.StateController.Setup(this);
            this.StatusController.Setup(this, this.statusData);
            this.MotionController = new ActorMotionController();
            this.MotionController.Setup(this, this.motionData);
            this.DirectorController.Setup(this, this.director);

            this.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    this.Disposables.Dispose();
                    this.Event.Dispose();
                });
        }

        void Start()
        {
            Actors.Add(this);

            if(this.gameObject.layer == Layer.Index.Player)
            {
                Players.Add(this);
            }
            else if(this.gameObject.layer == Layer.Index.Enemy)
            {
                Enemies.Add(this);
            }
            else
            {
                Debug.LogWarning($"{this.name}.{nameof(this.gameObject.layer)}はどの{typeof(Actor)}にも属していません");
            }

            GameEvent.OnSpawnedActorSubject().OnNext(this);
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
            this.transform.position = this.Animator.rootPosition;
        }

        public void SetRightEquipment(EquipmentController equipmentPrefab)
        {
            this.rightEquipment = equipmentPrefab.Attach(this);
        }

        public EquipmentController GetEquipmentController(HandType handType)
        {
            switch(handType)
            {
                case HandType.Left:
                    throw new NotImplementedException();
                case HandType.Right:
                    return this.rightEquipment;
                default:
                    Assert.IsTrue(false, $"{handType}は未実装です");
                    return null;
            }
        }
    }
}
