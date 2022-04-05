using ER.ActorControllers;
using ER.UIViews;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.UIPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FadePresenter : UIPresenter
    {
        [SerializeField]
        private FadeUIView fadeUIView = default;

        private void Awake()
        {
            this.fadeUIView.PlayInImmediate();
        }
    }
}
