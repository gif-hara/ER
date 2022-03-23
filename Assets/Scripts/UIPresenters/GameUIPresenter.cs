using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

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
        }

        [SerializeField]
        private UIAnimationController ingameHudAnimationController = default;

        [SerializeField]
        private UIAnimationController ingameMenuAnimationController = default;

        [SerializeField]
        private UIAnimationController changeEquipmentAnimationController = default;

        [SerializeField]
        private IngameRootMenuPresenter ingameRootMenuPresenter = default;

        private UIAnimationController currentRoot;

        private StateController<StateType> stateController;

        private void Awake()
        {
            this.stateController = new StateController<StateType>(StateType.Invalid);
            this.stateController.Set(StateType.Hud, this.OnEnterHud, null);
            this.stateController.Set(StateType.Menu, this.OnEnterMenu, null);
            this.stateController.Set(StateType.ChangeEquipment, this.OnEnterChangeEquipment, null);
            this.stateController.ChangeRequest(StateType.Hud);

            GameController.Instance.Event.OnRequestOpenIngameMenuSubject()
                .Subscribe(_ =>
                {
                    this.stateController.ChangeRequest(StateType.Menu);
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

            this.ingameRootMenuPresenter.Activate();
        }

        private void OnEnterChangeEquipment(StateType prev)
        {
            var inputAction = GameController.Instance.InputAction;
            inputAction.UI.Cancel.OnPerformedAsObservable()
                .Subscribe(_ => this.stateController.ChangeRequest(StateType.Menu))
                .AddTo(this.stateController.StateDisposables);

            this.ChangeCurrentRoot(this.changeEquipmentAnimationController);
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
