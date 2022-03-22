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
    public sealed class EnemyStatusPresenter : UIPresenter
    {
        [SerializeField]
        private RectTransform canvasTransform = default;

        [SerializeField]
        private Camera uiCamera = default;

        [SerializeField]
        private EnemyStatusUIView enemyStatusUIView = default;

        [SerializeField]
        private Vector2 offset = default;

        private Actor target;

        private Camera worldCamera;

        /// <summary>
        /// ヒットポイントを監視している<see cref="CompositeDisposable"/>
        /// </summary>
        private CompositeDisposable hitPointDisposables = new CompositeDisposable();

        private void Awake()
        {
            GameController.Instance.Event.OnSpawnedActorSubject()
                .Where(x => x.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.RegisterActorEvent(x);
                })
                .AddTo(this);

            GameController.Instance.Event.OnSpawnedGameCameraControllerSubject()
                .Subscribe(x =>
                {
                    this.worldCamera = x.ControlledCamera;
                })
                .AddTo(this);

            this.enemyStatusUIView.RootCanvasGroup.alpha = 0.0f;
        }

        private void LateUpdate()
        {
            if (this.target == null)
            {
                return;
            }
            Vector2 position;
            var screenPosition = RectTransformUtility.WorldToScreenPoint(this.worldCamera, this.target.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasTransform, screenPosition, this.uiCamera, out position);
            this.enemyStatusUIView.Root.localPosition = position + this.offset;
        }

        private void RegisterActorEvent(Actor actor)
        {
            actor.Broker.Receive<ActorEvent.OnBeginLookAt>()
                .Subscribe(x =>
                {
                    this.target = x.Target;
                    this.enemyStatusUIView.RootCanvasGroup.alpha = 1.0f;
                    this.enemyStatusUIView.EnemyName.text = this.target.StatusController.BaseStatus.LocalizedName;

                    this.target.StatusController.HitPointAsObservable()
                    .Subscribe(_ =>
                    {
                        this.enemyStatusUIView.HitPointSlider.value = this.target.StatusController.HitPointRate;
                    })
                    .AddTo(this.hitPointDisposables);
                })
                .AddTo(actor.Disposables);
            actor.Broker.Receive<ActorEvent.OnEndLookAt>()
                .Subscribe(_ =>
                {
                    this.target = null;
                    this.enemyStatusUIView.RootCanvasGroup.alpha = 0.0f;
                    this.hitPointDisposables.Clear();
                })
                .AddTo(actor.Disposables);
        }
    }
}
