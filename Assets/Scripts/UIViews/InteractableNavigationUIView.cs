using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InteractableNavigationUIView : UIView
    {
        [SerializeField]
        private UIAnimationController animationController = default;

        [SerializeField]
        private TextMeshProUGUI message = default;

        public UIAnimationController AnimationController => this.animationController;

        public string Message
        {
            set => this.message.text = value;
        }
    }
}
