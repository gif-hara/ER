using System;
using System.Collections;
using System.Collections.Generic;
using FancyScrollView;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FancyScrollView : FancyScrollRect<FancyCellInventoryItem, FancyScrollView.ERContext>
    {
        [SerializeField]
        private Scroller scroller = default;

        [SerializeField]
        private GameObject cellPrefab = default;

        [SerializeField, Range(0.0f, 1.0f)]
        private float jumpToAlignment = default;

        protected override GameObject CellPrefab => this.cellPrefab;

        protected override float CellSize
        {
            get
            {
                var rectTransform = (RectTransform)this.cellPrefab.transform;
                return rectTransform.rect.height;
            }
        }

        public ScrollDirection ScrollDirection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Func<(float ScrollSize, float ReuseMargin)> CalculateScrollSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private void Start()
        {
            this.scroller.OnValueChanged(base.UpdatePosition);
        }

        public void UpdateData(IList<FancyCellInventoryItem> items)
        {
            base.UpdateContents(items);
            scroller.SetTotalCount(items.Count);
        }

        public void JumpTo(int index)
        {
            this.JumpTo(index, this.jumpToAlignment);
            this.SetSelectedGameObject(index);
        }

        public IObservable<int> OnClickElementAsObservable()
        {
            return Observable.FromEvent<int>(x => this.Context.onClickAction += x, x => this.Context.onClickAction -= x);
        }

        private void SetSelectedGameObject(int index)
        {
            for (var i = 0; i < this.Context.Cells.Count; i++)
            {
                if (this.Context.Cells[i].Index == index)
                {
                    EventSystem.current.SetSelectedGameObject(this.Context.Selectables[i].gameObject);
                    break;
                }
            }
        }

        public sealed class ERContext : FancyScrollRectContext
        {
            public readonly List<FancyScrollRectCell<FancyCellInventoryItem, ERContext>> Cells = new List<FancyScrollRectCell<FancyCellInventoryItem, ERContext>>();

            public readonly List<Selectable> Selectables = new List<Selectable>();

            public Action<int> onClickAction;
        }
    }
}
