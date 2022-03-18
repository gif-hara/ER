using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnemyStatusUIView : UIView
    {

        [SerializeField]
        private CanvasGroup rootCanvasGroup = default;

        [SerializeField]
        private Transform root = default;

        [SerializeField]
        private Slider hitPointSlider = default;

        [SerializeField]
        private TextMeshProUGUI enemyName = default;

        public CanvasGroup RootCanvasGroup => this.rootCanvasGroup;
        
        public Transform Root => this.root;

        public Slider HitPointSlider => this.hitPointSlider;

        public TextMeshProUGUI EnemyName => this.enemyName;
    }
}
