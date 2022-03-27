using ER.ActorControllers;
using ER.UIViews;
using I2.Loc;
using System;
using System.Linq;
using System.Text;
using UniRx;
using UniRx.Triggers;
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

        private void Awake()
        {
        }

        public void Activate(IngameMenuType menuType)
        {
            var elements = this.ingameRootMenuUIView.CreateButtonElements(new Action<MenuButtonElement>[]
            {
                e =>
                {
                    e.Label.text = ScriptLocalization.Common.Equipment;
                    e.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenChangeEquipment.Get());
                    })
                    .AddTo(e.Button);
                },
                e =>
                {
                    e.Label.text = ScriptLocalization.Common.Inventory;
                    e.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        Debug.Log("TODO");
                    })
                    .AddTo(e.Button);
                },
                e =>
                {
                    e.Label.text = ScriptLocalization.Common.System;
                    e.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        Debug.Log("TODO");
                    })
                    .AddTo(e.Button);
                },
            });

            var selectables = elements.Select(x => x.Button).ToList();
            selectables.SetupVerticalNavigations();
            EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
        }

        public void Deactivate()
        {
            this.disposables.Clear();
        }
    }
}
