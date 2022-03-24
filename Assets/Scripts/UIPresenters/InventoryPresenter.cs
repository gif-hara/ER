using System;
using System.Collections.Generic;
using System.Linq;
using ER.ActorControllers;
using ER.UIViews;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace ER.UIPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InventoryPresenter : UIPresenter
    {
        [SerializeField]
        private InventoryUIView inventoryUIView = default;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        private List<Item> targetItems;

        private Action<Item> onSelectItemAction;

        private int currentIndex;

        public void Setup(List<Item> targetItems, Action<Item> onSelectItemAction)
        {
            this.targetItems = targetItems;
            this.onSelectItemAction = onSelectItemAction;
        }

        public void Activate()
        {
            var items = this.targetItems
                .Select(x => new FancyCellInventoryItem($"{x.MasterDataItem.LocalizedName}"))
                .ToArray();
            this.inventoryUIView.ScrollView.UpdateData(items);

            this.inventoryUIView.ScrollView.OnClickElementAsObservable()
                .Subscribe(x => onSelectItemAction(this.targetItems[x]))
                .AddTo(this.disposables);

            this.inventoryUIView.ScrollView.JumpTo(this.currentIndex);
            var inputAction = GameController.Instance.InputAction;
            inputAction.UI.Navigate.OnPerformedAsObservable()
                .Subscribe(x =>
                {
                    var value = x.ReadValue<Vector2>();
                    if (value.y >= 1.0f)
                    {
                        this.currentIndex--;
                        this.currentIndex = this.currentIndex < 0 ? this.targetItems.Count - 1 : this.currentIndex;
                        this.inventoryUIView.ScrollView.JumpTo(this.currentIndex);
                    }
                    else if (value.y <= -1.0f)
                    {
                        this.currentIndex++;
                        this.currentIndex = this.currentIndex >= this.targetItems.Count ? 0 : this.currentIndex;
                        this.inventoryUIView.ScrollView.JumpTo(this.currentIndex);
                    }
                })
                .AddTo(this.disposables);
        }

        public void Deactivate()
        {
            this.disposables.Clear();
        }
    }
}
