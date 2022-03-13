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
    public sealed class EnemyHitPointPresenter : UIPresenter
    {
        [SerializeField]
        private RectTransform canvasTransform = default;

        [SerializeField]
        private Camera uiCamera = default;

        [SerializeField]
        private EnemyHitPointUIView enemyHitPointUIView = default;

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
            GameEvent.OnSpawnedActorSubject()
                .Where(x => x.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.RegisterActorEvent(x);
                })
                .AddTo(this);

            GameEvent.OnSpawnedGameCameraController()
                .Subscribe(x =>
                {
                    this.worldCamera = x.ControlledCamera;
                })
                .AddTo(this);

            this.enemyHitPointUIView.HitPointRoot.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if(this.target == null)
            {
                return;
            }
            Vector2 position;
            var screenPosition = RectTransformUtility.WorldToScreenPoint(this.worldCamera, this.target.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasTransform, screenPosition, this.uiCamera, out position);
            this.enemyHitPointUIView.HitPointRoot.localPosition = position + this.offset;
        }

        private void RegisterActorEvent(Actor actor)
        {
            actor.Event.OnBeginLookAtSubject()
                .Subscribe(x =>
                {
                    this.target = x;
                    this.enemyHitPointUIView.HitPointRoot.gameObject.SetActive(true);

                    this.target.StatusController.HitPointAsObservable()
                    .Subscribe(hitPoint =>
                    {
                        this.enemyHitPointUIView.HitPointSlider.value = x.StatusController.HitPointRate;
                    })
                    .AddTo(this.hitPointDisposables);
                })
                .AddTo(actor.Disposables);
            actor.Event.OnEndLookAtSubject()
                .Subscribe(_ =>
                {
                    this.target = null;
                    this.enemyHitPointUIView.HitPointRoot.gameObject.SetActive(false);
                    this.hitPointDisposables.Clear();
                })
                .AddTo(actor.Disposables);
        }
    }
}
