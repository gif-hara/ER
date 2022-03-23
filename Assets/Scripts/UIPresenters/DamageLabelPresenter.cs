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
    public sealed class DamageLabelPresenter : UIPresenter
    {
        [SerializeField]
        private RectTransform canvasTransform = default;

        [SerializeField]
        private Camera uiCamera = default;

        [SerializeField]
        private DamageLabelUIView damageLabelUIView = default;

        private Camera worldCamera;

        private void Awake()
        {
            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
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
        }

        private void RegisterActorEvent(Actor actor)
        {
            actor.Broker.Receive<ActorEvent.OnTakedDamage>()
                .Subscribe(x =>
                {
                    var element = this.damageLabelUIView.CreateElement(x.Damage.ToString());
                    Vector2 position;
                    var screenPosition = RectTransformUtility.WorldToScreenPoint(this.worldCamera, actor.transform.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasTransform, screenPosition, this.uiCamera, out position);
                    element.transform.localPosition = position;
                })
                .AddTo(this);
        }
    }
}
