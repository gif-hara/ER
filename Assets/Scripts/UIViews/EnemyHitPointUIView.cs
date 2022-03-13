using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnemyHitPointUIView : UIView
    {
        [SerializeField]
        private Transform hitPointRoot = default;

        [SerializeField]
        private Slider hitPointSlider = default;

        public Transform HitPointRoot => this.hitPointRoot;

        public Slider HitPointSlider => this.hitPointSlider;
    }
}
