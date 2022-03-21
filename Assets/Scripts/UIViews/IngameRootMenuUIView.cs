using System;
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
    public sealed class IngameRootMenuUIView : UIView
    {
        [SerializeField]
        private IngameRootMenuButtonElement buttonElementPrefab = default;

        [SerializeField]
        private Transform buttonElementParent = default;

        public void ClearButtonElements()
        {
            for (var i = 0; i < this.buttonElementParent.childCount; i++)
            {
                Destroy(this.buttonElementParent.GetChild(i).gameObject);
            }
        }

        public void CreateButtonElement(string label, Action onClickAction)
        {
            var element = Instantiate(this.buttonElementPrefab, this.buttonElementParent, false);
            element.Label.text = label;
            element.Button.OnClickAsObservable()
                .Subscribe(_ => onClickAction())
                .AddTo(element);
        }
    }
}
