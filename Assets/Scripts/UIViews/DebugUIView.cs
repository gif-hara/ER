using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DebugUIView : UIView
    {
        [SerializeField]
        private TextMeshProUGUI text = default;

        public TextMeshProUGUI Text => this.text;
    }
}
