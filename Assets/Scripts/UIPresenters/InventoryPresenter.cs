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

        private FancyCellInventoryItem[] items;

        private int currentIndex;

        public void Activate()
        {
            this.items = Enumerable.Range(0, 50)
                .Select(x => new FancyCellInventoryItem($"Cell {x}"))
                .ToArray();
            this.inventoryUIView.ScrollView.UpdateData(this.items);

            this.inventoryUIView.ScrollView.OnClickElementAsObservable()
                .Subscribe(x => Debug.Log(x))
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
                        this.currentIndex = this.currentIndex < 0 ? this.items.Length - 1 : this.currentIndex;
                        this.inventoryUIView.ScrollView.JumpTo(this.currentIndex);
                    }
                    else if (value.y <= -1.0f)
                    {
                        this.currentIndex++;
                        this.currentIndex = this.currentIndex >= this.items.Length ? 0 : this.currentIndex;
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
