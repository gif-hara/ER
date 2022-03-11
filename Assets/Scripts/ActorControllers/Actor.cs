using ER.EquipmentSystems;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Actor : MonoBehaviour, IActor
    {
        [SerializeField]
        private ActorStatus status = default;

        [SerializeField]
        private ActorMotionData motionData = default;

        [SerializeField]
        private ActorAnimationParameter animationParameter = default;

        private readonly ActorStatusController statusController = new ActorStatusController();

        public ActorStateController StateController { get; } = new ActorStateController();

        private EquipmentController rightEquipment;

        public Animator Animator { get; private set; }

        public ActorEvent Event { get; private set; }

        public ActorAnimationParameter AnimationParameter => this.animationParameter;

        public ActorMotionController MotionController { get; private set; }

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        void Awake()
        {
            this.Animator = this.GetComponent<Animator>();
            this.Event = new ActorEvent();
            this.StateController.Setup(this, this.disposable);
            this.statusController.Setup(this, this.status, this.disposable);
            this.MotionController = new ActorMotionController();
            this.MotionController.Setup(this, this.motionData, this.disposable);

            this.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    this.disposable.Dispose();
                    this.Event.Dispose();
                });

            this.Event.OnDeadSubject()
                .Subscribe(_ =>
                {
                    Destroy(this.gameObject);
                })
                .AddTo(this.disposable);
        }

        void OnAnimatorMove()
        {
            this.transform.position = this.Animator.rootPosition;
        }

        public void SetRightEquipment(EquipmentController equipmentPrefab)
        {
            this.rightEquipment = equipmentPrefab.Attach(this);
        }
    }
}
