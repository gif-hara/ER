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
    public sealed class InventoryUIView : UIView
    {
        [SerializeField]
        private FancyScrollView scrollView = default;

        public FancyScrollView ScrollView => this.scrollView;
    }
}
