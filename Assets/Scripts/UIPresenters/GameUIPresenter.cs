using UnityEngine;
using UniRx;
using UniRx.Triggers;
using ER.ActorControllers;
using UnityEngine.InputSystem;

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
            FromStartButtonMenu,
            ChangeEquipment,
            Inventory,
            CheckPointMenu,
            InputTutorial,
            ThankYouForPlaying,
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
        private UIAnimationController inputTutorialAnimationController = default;

        [SerializeField]
        private IngameRootMenuPresenter ingameRootMenuPresenter = default;

        [SerializeField]
        private ChangeEquipmentPresenter changeEquipmentPresenter = default;

        [SerializeField]
        private InventoryPresenter inventoryPresenter = default;

        [SerializeField]
        private UIAnimationController thankYouForPlayingAnimationController = default;

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
            this.inputTutorialAnimationController.PlayImmediate(false);
            this.thankYouForPlayingAnimationController.PlayImmediate(false);

            this.stateController = new StateController<StateType>(StateType.Invalid);
            this.stateController.Set(StateType.Hud, this.OnEnterHud, null);
            this.stateController.Set(StateType.FromStartButtonMenu, this.OnEnterFromStartButtonMenu, this.OnExitFromStartButtonMenu);
            this.stateController.Set(StateType.ChangeEquipment, this.OnEnterChangeEquipment, this.OnExitChangeEquipment);
            this.stateController.Set(StateType.Inventory, this.OnEnterInventory, this.OnExitInventory);
            this.stateController.Set(StateType.CheckPointMenu, this.OnEnterCheckPointMenu, this.OnExitCheckPointMenu);
            this.stateController.Set(StateType.InputTutorial, this.OnEnterInputTutorial, null);
            this.stateController.Set(StateType.ThankYouForPlaying, this.OnEnterThankYouForPlaying, null);
            this.stateController.ChangeRequest(StateType.Hud);

            GameController.Instance.Broker.Receive<GameEvent.OnRequestOpenIngameMenu>()
                .Subscribe(x =>
                {
                    this.requestMenuType = x.IngameMenuType;
                    this.stateController.ChangeRequest(StateType.FromStartButtonMenu);
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

            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
                .Where(x => x.SpawnedActor.gameObject.layer == Layer.Index.Player)
                .Subscribe(x => this.RegisterActorEvent(x.SpawnedActor))
                .AddTo(this);

            GameController.Instance.Broker.Receive<GameEvent.OnRequestOpenInputTutorial>()
                .Subscribe(_ =>
                {
                    this.stateController.ChangeRequest(StateType.InputTutorial);
                })
                .AddTo(this);

            GameController.Instance.Broker.Receive<GameEvent.OnRequestOpenThankYouForPlaying>()
                .Subscribe(_ =>
                {
                    this.stateController.ChangeRequest(StateType.ThankYouForPlaying);
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    this.stateController.Update();
                });
        }

        private void RegisterActorEvent(Actor actor)
        {
            actor.Broker.Receive<ActorEvent.OnInteractedCheckPoint>()
                .Where(x => x.CanOpenMenu)
                .Subscribe(_ =>
                {
                    this.requestMenuType = IngameMenuType.CheckPoint;
                    this.stateController.ChangeRequest(StateType.CheckPointMenu);
                })
                .AddTo(this);
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

        private void OnEnterFromStartButtonMenu(StateType prev)
        {
            EnableUIInputAction();

            var inputAction = GameController.Instance.InputAction;
            inputAction.UI.Cancel.OnPerformedAsObservable()
                .Subscribe(_ => this.stateController.ChangeRequest(StateType.Hud))
                .AddTo(this.stateController.StateDisposables);

            this.ChangeCurrentRoot(this.ingameMenuAnimationController);

            this.ingameRootMenuPresenter.Activate(this.requestMenuType);
        }

        private void OnExitFromStartButtonMenu(StateType next)
        {
            this.ingameRootMenuPresenter.Deactivate();
        }

        private void OnEnterChangeEquipment(StateType prev)
        {
            var inputAction = GameController.Instance.InputAction;
            inputAction.UI.Cancel.OnPerformedAsObservable()
                .Subscribe(_ => this.stateController.ChangeRequest(StateType.FromStartButtonMenu))
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

        private void OnEnterCheckPointMenu(StateType prev)
        {
            EnableUIInputAction();

            var inputAction = GameController.Instance.InputAction;
            inputAction.UI.Cancel.OnPerformedAsObservable()
                .Subscribe(_ => this.stateController.ChangeRequest(StateType.Hud))
                .AddTo(this.stateController.StateDisposables);

            this.ChangeCurrentRoot(this.ingameMenuAnimationController);

            this.ingameRootMenuPresenter.Activate(this.requestMenuType);
        }

        private void OnExitCheckPointMenu(StateType next)
        {
            this.ingameRootMenuPresenter.Deactivate();
        }

        private void OnEnterInputTutorial(StateType prev)
        {
            EnableUIInputAction();
            var inputAction = GameController.Instance.InputAction;
            inputAction.UI.Cancel.OnPerformedAsObservable()
                .Subscribe(_ => this.stateController.ChangeRequest(StateType.Hud))
                .AddTo(this.stateController.StateDisposables);
            
            this.ChangeCurrentRoot(this.inputTutorialAnimationController);
        }

        private void OnEnterThankYouForPlaying(StateType prev)
        {
            EnableUIInputAction();
            this.ChangeCurrentRoot(this.thankYouForPlayingAnimationController);
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
