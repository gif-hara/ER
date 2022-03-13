using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LockonReticleUIView : UIView
    {
        [SerializeField]
        private Transform reticle = default;

        public Transform Reticle => this.reticle;
    }
}
