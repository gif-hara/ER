using ER.EquipmentSystems;
using UnityEngine.Assertions;
using UniRx;
using ER.MasterDataSystem;
using UnityEngine;
using System.Collections.Generic;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorEquipmentController
    {
        private Actor actor;

        public Hand RightHand { get; } = new Hand();

        public Hand LeftHand { get; } = new Hand();

        public EquipmentController GuardingEquipmentController { get; private set; }

        public MasterDataArmor.Record Head { get; private set; }

        public MasterDataArmor.Record Torso { get; private set; }

        public MasterDataArmor.Record Arm { get; private set; }

        public MasterDataArmor.Record Leg { get; private set; }

        /// <summary>
        /// 左手行動のリクエストが来ているか
        /// </summary>
        public bool IsLeftRequest { get; private set; }

        public void Setup(Actor actor)
        {
            this.actor = actor;
            this.RightHand.Setup(actor);
            this.LeftHand.Setup(actor);

            this.actor.Event.OnBeginLeftEquipmentSubject()
                .Subscribe(_ => this.IsLeftRequest = true)
                .AddTo(actor.Disposables);

            this.actor.Event.OnEndLeftEquipmentSubject()
                .Subscribe(_ => this.IsLeftRequest = false)
                .AddTo(actor.Disposables);
        }

        public void BeginGuard(EquipmentController equipmentController)
        {
            this.GuardingEquipmentController = equipmentController;
        }

        public void EndGuard()
        {
            this.GuardingEquipmentController = null;
        }

        public MasterDataArmor.Record GetArmor(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.Head:
                    return this.Head;
                case ArmorType.Torso:
                    return this.Torso;
                case ArmorType.Arm:
                    return this.Arm;
                case ArmorType.Leg:
                    return this.Leg;
                default:
                    Assert.IsTrue(false, $"{armorType}は未対応です");
                    return null;
            }
        }

        public void SetArmor(ArmorType armorType, string masterDataArmorId)
        {
            if (!MasterDataArmor.Contains(masterDataArmorId))
            {
                return;
            }

            var masterDataArmor = MasterDataArmor.Get(masterDataArmorId);
            switch (armorType)
            {
                case ArmorType.Head:
                    this.Head = masterDataArmor;
                    break;
                case ArmorType.Torso:
                    this.Torso = masterDataArmor;
                    break;
                case ArmorType.Arm:
                    this.Arm = masterDataArmor;
                    break;
                case ArmorType.Leg:
                    this.Leg = masterDataArmor;
                    break;
                default:
                    Assert.IsTrue(false, $"{armorType}は未対応です");
                    break;
            }
        }

        public int GetDefense(AttackAttributeType attackAttributeType)
        {
            var result = 0;
            if (this.Head != null) result += this.Head.GetDefense(attackAttributeType);
            if (this.Torso != null) result += this.Torso.GetDefense(attackAttributeType);
            if (this.Arm != null) result += this.Arm.GetDefense(attackAttributeType);
            if (this.Leg != null) result += this.Leg.GetDefense(attackAttributeType);

            return result;
        }

        public EquipmentController GetEquipmentController(HandType handType)
        {
            switch (handType)
            {
                case HandType.Left:
                    return this.LeftHand.CurrentEquipmentController;
                case HandType.Right:
                    return this.RightHand.CurrentEquipmentController;
                default:
                    Assert.IsTrue(false, $"{handType}は未実装です");
                    return null;
            }
        }

        public class Hand
        {
            public EquipmentController CurrentEquipmentController { get; private set; }

            private int index;

            private EquipmentController[] equipmentPrefabs = new EquipmentController[Define.EquipmentableNumber];

            private Actor actor;

            public void Setup(Actor actor)
            {
                this.actor = actor;
            }

            public void Attach(EquipmentController equipmentPrefab, IEquipmentData equipmentData)
            {
                if (this.CurrentEquipmentController != null)
                {
                    Object.Destroy(this.CurrentEquipmentController.gameObject);
                }

                this.CurrentEquipmentController = equipmentPrefab.Attach(this.actor, equipmentData);
            }
        }
    }
}
