using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ER.UIViews
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DamageLabelUIView : UIView
    {
        [SerializeField]
        private DamageLabelElement elementPrefab = default;

        public DamageLabelElement CreateElement(string text)
        {
            var element = Instantiate(this.elementPrefab, this.transform);
            element.Setup(text);

            return element;
        }
    }
}
