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
        }

        [SerializeField]
        private UIAnimationController ingameHudAnimationController = default;

        [SerializeField]
        private UIAnimationController ingameMenuAnimationController = default;

        [SerializeField]
        private IngameRootMenuPresenter ingameRootMenuPresenter = default;

        private UIAnimationController currentRoot;

        private StateController<StateType> stateController;

        private void Awake()
        {
            this.stateController = new StateController<StateType>(StateType.Invalid);
            this.stateController.Set(StateType.Hud, this.OnEnterHud, null);
            this.stateController.Set(StateType.Menu, this.OnEnterMenu, null);

            this.ChangeCurrentRoot(this.ingameHudAnimationController);

            GameEvent.OnRequestOpenIngameMenuSubject()
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
            GameController.Instance.InputAction.Player.Enable();
            GameController.Instance.InputAction.UI.Disable();
            this.ChangeCurrentRoot(this.ingameHudAnimationController);
        }

        private void OnEnterMenu(StateType prev)
        {
            GameController.Instance.InputAction.Player.Disable();
            GameController.Instance.InputAction.UI.Enable();
            this.ChangeCurrentRoot(this.ingameMenuAnimationController);

            this.ingameRootMenuPresenter.Activate();
        }
    }
}
