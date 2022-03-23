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
        private List<MenuButtonElement> rightEquipmentButtonElements = default;

        [SerializeField]
        private List<MenuButtonElement> leftEquipmentButtonElements = default;

        [SerializeField]
        private MenuButtonElement headButtonElement = default;

        [SerializeField]
        private MenuButtonElement torsoButtonElement = default;

        [SerializeField]
        private MenuButtonElement armButtonElement = default;

        [SerializeField]
        private MenuButtonElement legButtonElement = default;

        [SerializeField]
        private TextMeshProUGUI information = default;

        public TextMeshProUGUI Information => this.information;

        public MenuButtonElement GetRightEquipmentButtonElement(int index)
        {
            return this.rightEquipmentButtonElements[index];
        }

        public MenuButtonElement GetLeftEquipmentButtonElement(int index)
        {
            return this.leftEquipmentButtonElements[index];
        }

        public MenuButtonElement GetArmorButtonElement(ArmorType armorType)
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

        public Button[] GetAllButtons()
        {
            return this.GetComponentsInChildren<Button>();
        }
    }
}
