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
    public sealed class IngameRootMenuUIView : UIView
    {
        [SerializeField]
        private MenuButtonElement buttonElementPrefab = default;

        [SerializeField]
        private Transform buttonElementParent = default;

        private void ClearButtonElements()
        {
            for (var i = 0; i < this.buttonElementParent.childCount; i++)
            {
                Destroy(this.buttonElementParent.GetChild(i).gameObject);
            }
        }

        public List<MenuButtonElement> CreateButtonElements(params Action<MenuButtonElement>[] setupActions)
        {
            var result = new List<MenuButtonElement>();
            this.ClearButtonElements();
            foreach (var i in setupActions)
            {
                var element = Instantiate(this.buttonElementPrefab, this.buttonElementParent, false);
                i(element);
                result.Add(element);
            }

            return result;
        }
    }
}
