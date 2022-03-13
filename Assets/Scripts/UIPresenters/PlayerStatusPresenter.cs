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
    public sealed class PlayerStatusPresenter : UIPresenter
    {
        [SerializeField]
        private PlayerStatusUIView playerStatusUIView = default;

        private void Awake()
        {
            GameEvent.OnSpawnedActorSubject()
                .Where(x => x.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.RegisterActorEvent(x);
                })
                .AddTo(this);
        }

        private void RegisterActorEvent(Actor actor)
        {
            actor.StatusController.HitPointAsObservable()
                .Subscribe(_ =>
                {
                    this.playerStatusUIView.HitPointSlider.value = actor.StatusController.HitPointRate;
                })
                .AddTo(actor.Disposables);
        }
    }
}
