using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PlayerStatusUIView : UIView
    {
        [SerializeField]
        private Slider hitPointSlider = default;

        [SerializeField]
        private HandStatus leftHandStatus = default;

        [SerializeField]
        private HandStatus rightHandStatus = default;

        [SerializeField]
        private TextMeshProUGUI experience = default;

        public Slider HitPointSlider => this.hitPointSlider;

        public HandStatus LeftHandStatus => this.leftHandStatus;

        public HandStatus RightHandStatus => this.rightHandStatus;

        public TextMeshProUGUI Experience => this.experience;

        [Serializable]
        public class HandStatus
        {
            [SerializeField]
            private TextMeshProUGUI equipmentName = default;
            public TextMeshProUGUI EquipmentName => this.equipmentName;
        }
    }
}
