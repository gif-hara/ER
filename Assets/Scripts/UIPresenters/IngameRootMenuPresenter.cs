using ER.UIViews;
using I2.Loc;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace ER.UIPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IngameRootMenuPresenter : UIPresenter
    {
        [SerializeField]
        private IngameRootMenuUIView ingameRootMenuUIView = default;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public void Activate(IngameMenuType menuType)
        {
            var elements = this.ingameRootMenuUIView.CreateButtonElements(GetButtonMenuActions(menuType));

            var selectables = elements.Select(x => x.Button).ToList();
            selectables.SetupVerticalNavigations();
            EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
        }

        public void Deactivate()
        {
            this.disposables.Clear();
        }

        private Action<MenuButtonElement>[] GetButtonMenuActions(IngameMenuType menuType)
        {
            switch (menuType)
            {
                case IngameMenuType.FromStartButton:
                    return this.CreateFromStartButtonMenuActions();
                case IngameMenuType.CheckPoint:
                    return this.CreateCheckPointMenuActions();
                default:
                    Assert.IsTrue(false, $"{menuType}は未対応です");
                    return null;
            }
        }

        private Action<MenuButtonElement>[] CreateFromStartButtonMenuActions()
        {
            return new Action<MenuButtonElement>[]
            {
                e =>
                {
                    e.Label.text = ScriptLocalization.Common.Equipment;
                    e.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenChangeEquipment.Get());
                    })
                    .AddTo(e.Button)
                    .AddTo(this.disposables);
                },
                e =>
                {
                    e.Label.text = ScriptLocalization.Common.Inventory;
                    e.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        Debug.Log("TODO");
                    })
                    .AddTo(e.Button)
                    .AddTo(this.disposables);
                },
                e =>
                {
                    e.Label.text = ScriptLocalization.Common.System;
                    e.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        Debug.Log("TODO");
                    })
                    .AddTo(e.Button)
                    .AddTo(this.disposables);
                },
            };
        }

        private Action<MenuButtonElement>[] CreateCheckPointMenuActions()
        {
            return new Action<MenuButtonElement>[]
            {
                e =>
                {
                    e.Label.text = "装備強化";
                    e.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        Debug.Log("TODO");
                    })
                    .AddTo(e.Button);
                },
                e =>
                {
                    e.Label.text = "加護強化";
                    e.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        Debug.Log("TODO");
                    })
                    .AddTo(e.Button);
                },
                e =>
                {
                    e.Label.text = "セーブ";
                    e.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        Debug.Log("TODO");
                    })
                    .AddTo(e.Button);
                },
            };
        }
    }
}
