using ER.EquipmentSystems;
using UnityEngine.Assertions;
using UniRx;
using ER.MasterDataSystem;
using UnityEngine;

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

        public MasterDataArmor Head { get; private set; }

        public MasterDataArmor Torso { get; private set; }

        public MasterDataArmor Arm { get; private set; }

        public MasterDataArmor Leg { get; private set; }

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
            if (this.rightEquipemntContoller != null)
            {
                Object.Destroy(this.rightEquipemntContoller.gameObject);
            }

            this.rightEquipemntContoller = equipmentPrefab.Attach(this.actor, equipmentData);
        }

        public void AttachLeftEquipment(EquipmentController equipmentPrefab, IEquipmentData equipmentData)
        {
            if (this.leftEquipmentController != null)
            {
                Object.Destroy(this.leftEquipmentController.gameObject);
            }

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
