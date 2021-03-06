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
    public sealed class LockonReticlePresenter : UIPresenter
    {
        [SerializeField]
        private RectTransform canvasTransform = default;

        [SerializeField]
        private Camera uiCamera = default;

        [SerializeField]
        private LockonReticleUIView lockonReticleUIView = default;

        private Transform target;

        private Camera worldCamera;

        private void Awake()
        {
            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
                .Where(x => x.SpawnedActor.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.RegisterActorEvent(x.SpawnedActor);
                })
                .AddTo(this);

            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedGameCameraController>()
                .Subscribe(x =>
                {
                    this.worldCamera = x.SpawnedGameCameraController.ControlledCamera;
                })
                .AddTo(this);

            this.lockonReticleUIView.Reticle.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (this.target == null)
            {
                return;
            }
            Vector2 position;
            var screenPosition = RectTransformUtility.WorldToScreenPoint(this.worldCamera, this.target.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasTransform, screenPosition, this.uiCamera, out position);
            this.lockonReticleUIView.Reticle.localPosition = position;
        }

        private void RegisterActorEvent(Actor actor)
        {
            actor.Broker.Receive<ActorEvent.OnBeginLookAt>()
                .Subscribe(x =>
                {
                    this.target = x.Target.transform;
                    this.lockonReticleUIView.Reticle.gameObject.SetActive(true);
                })
                .AddTo(actor.Disposables);
            actor.Broker.Receive<ActorEvent.OnEndLookAt>()
                .Subscribe(_ =>
                {
                    this.target = null;
                    this.lockonReticleUIView.Reticle.gameObject.SetActive(false);
                })
                .AddTo(actor.Disposables);
        }
    }
}
