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
    public sealed class AcquiredItemNotificationUIView : UIView
    {
        [SerializeField]
        private AcquiredItemNotificationElement elementPrefab = default;

        [SerializeField]
        private Transform elementParent = default;

        public void CreateElement(string message)
        {
            var element = Instantiate(this.elementPrefab, this.elementParent);
            element.Setup(message);
        }
    }
}
