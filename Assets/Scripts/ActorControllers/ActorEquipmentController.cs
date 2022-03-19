using ER.EquipmentSystems;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorEquipmentController
    {
        private Actor actor;

        private EquipmentController rightEquipemntContoller;

        private EquipmentController leftEquipmentController;

        public EquipmentController GuardingEquipmentController { get; private set; }

        /// <summary>
        /// 左手行動のリクエストが来ているか
        /// </summary>
        public bool IsLeftRequest { get; private set; }

        public void Setup(Actor actor)
        {
            this.actor = actor;

            this.actor.Event.OnBeginLeftEquipmentSubject()
                .Subscribe(_ => this.IsLeftRequest = true)
                .AddTo(actor.Disposables);

            this.actor.Event.OnEndLeftEquipmentSubject()
                .Subscribe(_ => this.IsLeftRequest = false)
                .AddTo(actor.Disposables);
        }

        public void AttachRightEquipment(EquipmentController equipmentPrefab, IEquipmentData equipmentData)
        {
            this.rightEquipemntContoller = equipmentPrefab.Attach(this.actor, equipmentData);
        }

        public void AttachLeftEquipment(EquipmentController equipmentPrefab, IEquipmentData equipmentData)
        {
            this.leftEquipmentController = equipmentPrefab.Attach(this.actor, equipmentData);
        }

        public void BeginGuard(EquipmentController equipmentController)
        {
            this.GuardingEquipmentController = equipmentController;
        }

        public void EndGuard()
        {
            this.GuardingEquipmentController = null;
        }

        public EquipmentController GetEquipmentController(HandType handType)
        {
            switch (handType)
            {
                case HandType.Left:
                    return this.leftEquipmentController;
                case HandType.Right:
                    return this.rightEquipemntContoller;
                default:
                    Assert.IsTrue(false, $"{handType}は未実装です");
                    return null;
            }
        }
    }
}
