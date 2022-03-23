using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChangeEquipmentUIView : UIView
    {
        [SerializeField]
        private List<IngameRootMenuButtonElement> rightEquipmentButtonElements = default;

        [SerializeField]
        private List<IngameRootMenuButtonElement> leftEquipmentButtonElements = default;

        [SerializeField]
        private IngameRootMenuButtonElement headButtonElement = default;

        [SerializeField]
        private IngameRootMenuButtonElement torsoButtonElement = default;

        [SerializeField]
        private IngameRootMenuButtonElement armButtonElement = default;

        [SerializeField]
        private IngameRootMenuButtonElement legButtonElement = default;

        [SerializeField]
        private TextMeshProUGUI information = default;

        public TextMeshProUGUI Information => this.information;

        public IngameRootMenuButtonElement GetRightEquipmentButtonElement(int index)
        {
            return this.rightEquipmentButtonElements[index];
        }

        public IngameRootMenuButtonElement GetLeftEquipmentButtonElement(int index)
        {
            return this.leftEquipmentButtonElements[index];
        }

        public IngameRootMenuButtonElement GetArmorButtonElement(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.Head:
                    return this.headButtonElement;
                case ArmorType.Torso:
                    return this.torsoButtonElement;
                case ArmorType.Arm:
                    return this.armButtonElement;
                case ArmorType.Leg:
                    return this.legButtonElement;
                default:
                    Assert.IsTrue(false, $"{armorType}は未対応です");
                    return null;
            }
        }
    }
}
