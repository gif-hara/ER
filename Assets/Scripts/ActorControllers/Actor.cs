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

        private readonly ActorStatusController statusController = new ActorStatusController();

        private EquipmentController rightEquipment;

        public Animator Animator { get; private set; }

        public ActorEvent Event { get; private set; }

        void Awake()
        {
            this.Animator = this.GetComponent<Animator>();
            this.Event = new ActorEvent();
            this.statusController.Setup(this.status);

            this.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    this.Event.Dispose();
                });
        }

        void OnAnimatorMove()
        {
            this.transform.position = this.Animator.rootPosition;
        }

        public void SetRightEquipment(EquipmentController equipmentPrefab)
        {
            this.rightEquipment = equipmentPrefab.Attach(this);
        }

        public void OnCollisionOpponentAttack(EquipmentController equipmentController)
        {
            Destroy(this.gameObject);
        }
    }
}
