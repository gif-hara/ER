using System;
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

        public static FadePresenter Instance { get; private set; }

        private void Awake()
        {
            this.fadeUIView.PlayInImmediate();
            Instance = this;
        }

        public IObservable<Unit> PlayInAsync()
        {
            return this.fadeUIView.PlayInAsync();
        }

        public IObservable<Unit> PlayOutAsync()
        {
            return this.fadeUIView.PlayOutAsync();
        }
    }
}
