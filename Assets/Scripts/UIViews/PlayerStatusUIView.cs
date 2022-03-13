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

        public Slider HitPointSlider => this.hitPointSlider;
    }
}
