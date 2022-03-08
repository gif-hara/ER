using ER.EquipmentSystems;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Actor : MonoBehaviour, IActor
    {
        private readonly Subject<Unit> beginRightEquipmentSubject = new Subject<Unit>();

        private readonly Subject<Unit> endRightEquipmentSubject = new Subject<Unit>();

        private EquipmentController rightEquipment;

        public Animator Animator { get; private set; }

        public IObservable<Unit> OnBeginRightEquipmentAsObservable() => this.beginRightEquipmentSubject;

        public IObservable<Unit> OnEndRightEquipmentAsObservable() => this.endRightEquipmentSubject;

        void Awake()
        {
            this.Animator = this.GetComponent<Animator>();
        }

        public void InvokeBeginRightEquipment()
        {
            this.beginRightEquipmentSubject.OnNext(Unit.Default);
        }

        public void InvokeEndRightEquipment()
        {
            this.endRightEquipmentSubject.OnNext(Unit.Default);
        }

        public void SetRightEquipment(EquipmentController equipmentPrefab)
        {
            this.rightEquipment = equipmentPrefab.Attach(this);
        }

        public void OnCollisionBullet(BulletController bulletController)
        {
            Destroy(this.gameObject);
        }
    }
}
