using System;
using FancyScrollView;
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
    public sealed class MenuButtonElement : FancyScrollRectCell<FancyCellInventoryItem, FancyScrollView.ERContext>
    {
        [SerializeField]
        private TextMeshProUGUI label = default;

        [SerializeField]
        private Button button = default;

        public TextMeshProUGUI Label => this.label;

        public Button Button => this.button;

        public override void Initialize()
        {
            this.Context.Cells.Add(this);
            this.Context.Selectables.Add(this.Button);
            this.Button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    this.Context.onClickAction(this.Index);
                })
                .AddTo(this);
        }

        public override void UpdateContent(FancyCellInventoryItem itemData)
        {
            this.Label.text = itemData.Message;
        }
    }
}
