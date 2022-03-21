using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace ER.UIPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameUIPresenter : UIPresenter
    {
        [SerializeField]
        private UIAnimationController ingameAreaAnimationController = default;

        private void Awake()
        {
            GameEvent.OnRequestOpenIngameMenuSubject()
                .Subscribe(_ =>
                {
                    GameController.Instance.InputAction.Player.Disable();
                    GameController.Instance.InputAction.UI.Enable();

                    this.ingameAreaAnimationController.Play(false);
                })
                .AddTo(this);
        }
    }
}
