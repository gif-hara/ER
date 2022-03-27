using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace ER.UIPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameUIPresenter : UIPresenter
    {
        private enum StateType
        {
            Invalid,
            Hud,
            Menu,
            ChangeEquipment,
            Inventory,
        }

        [SerializeField]
        private UIAnimationController ingameHudAnimationController = default;

        [SerializeField]
        private UIAnimationController ingameMenuAnimationController = default;

        [SerializeField]
        private UIAnimationController changeEquipmentAnimationController = default;

        [SerializeField]
        private UIAnimationController inventoryAnimationController = default;

        [SerializeField]
        private IngameRootMenuPresenter ingameRootMenuPresenter = default;

        [SerializeField]
        private ChangeEquipmentPresenter changeEquipmentPresenter = default;

        [SerializeField]
        private InventoryPresenter inventoryPresenter = default;

        private UIAnimationController currentRoot;

        private StateController<StateType> stateController;

        /// <summary>
        /// 次に開くメニュータイプ
        /// </summary>
        private IngameMenuType requestMenuType;

        private void Awake()
        {
            this.ingameHudAnimationController.PlayImmediate(false);
            this.ingameMenuAnimationController.PlayImmediate(false);
            this.changeEquipmentAnimationController.PlayImmediate(false);
            this.inventoryAnimationController.PlayImmediate(false);

            this.stateController = new StateController<StateType>(StateType.Invalid);
            this.stateController.Set(StateType.Hud, this.OnEnterHud, null);
            this.stateController.Set(StateType.Menu, this.OnEnterMenu, null);
            this.stateController.Set(StateType.ChangeEquipment, this.OnEnterChangeEquipment, this.OnExitChangeEquipment);
            this.stateController.Set(StateType.Inventory, this.OnEnterInventory, OnExitInventory);
            this.stateController.ChangeRequest(StateType.Hud);

            GameController.Instance.Broker.Receive<GameEvent.OnRequestOpenIngameMenu>()
                .Subscribe(x =>
                {
                    this.requestMenuType = x.IngameMenuType;
                    this.stateController.ChangeRequest(StateType.Menu);
                })
                .AddTo(this);

            GameController.Instance.Broker.Receive<GameEvent.OnRequestOpenChangeEquipment>()
                .Subscribe(_ => this.stateController.ChangeRequest(StateType.ChangeEquipment))
                .AddTo(this);

            GameController.Instance.Broker.Receive<GameEvent.OnRequestOpenInventory>()
                .Subscribe(x =>
                {
                    this.inventoryPresenter.Setup(x.TargetItems, x.OnSelectItemAction);
                    this.stateController.ChangeRequest(StateType.Inventory);
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    this.stateController.Update();
                });
        }

        private void ChangeCurrentRoot(UIAnimationController nextRoot)
        {
            if (this.currentRoot != null)
            {
                this.currentRoot.Play(false);
            }

            this.currentRoot = nextRoot;
            this.currentRoot.Play(true);
        }

        private void OnEnterHud(StateType prev)
        {
            EnablePlayerInputAction();
            this.ChangeCurrentRoot(this.ingameHudAnimationController);
        }

        private void OnEnterMenu(StateType prev)
        {
            EnableUIInputAction();

            var inputAction = GameController.Instance.InputAction;
            inputAction.UI.Cancel.OnPerformedAsObservable()
                .Subscribe(_ => this.stateController.ChangeRequest(StateType.Hud))
                .AddTo(this.stateController.StateDisposables);

            this.ChangeCurrentRoot(this.ingameMenuAnimationController);

            this.ingameRootMenuPresenter.Activate(this.requestMenuType);
        }

        private void OnEnterChangeEquipment(StateType prev)
        {
            var inputAction = GameController.Instance.InputAction;
            inputAction.UI.Cancel.OnPerformedAsObservable()
                .Subscribe(_ => this.stateController.ChangeRequest(StateType.Menu))
                .AddTo(this.stateController.StateDisposables);

            this.ChangeCurrentRoot(this.changeEquipmentAnimationController);

            this.changeEquipmentPresenter.Activate();
        }

        private void OnExitChangeEquipment(StateType next)
        {
            this.changeEquipmentPresenter.Deactivate();
        }

        private void OnEnterInventory(StateType prev)
        {
            var inputAction = GameController.Instance.InputAction;
            inputAction.UI.Cancel.OnPerformedAsObservable()
                .Subscribe(_ => this.stateController.ChangeRequest(prev))
                .AddTo(this.stateController.StateDisposables);

            this.ChangeCurrentRoot(this.inventoryAnimationController);

            this.inventoryPresenter.Activate();
        }

        private void OnExitInventory(StateType next)
        {
            this.inventoryPresenter.Deactivate();
        }

        private void EnableUIInputAction()
        {
            var inputAction = GameController.Instance.InputAction;
            inputAction.Player.Disable();
            inputAction.UI.Enable();
        }

        private void EnablePlayerInputAction()
        {
            var inputAction = GameController.Instance.InputAction;
            inputAction.Player.Enable();
            inputAction.UI.Disable();
        }
    }
}
