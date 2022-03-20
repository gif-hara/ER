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
            GameEvent.OnSpawnedActorSubject()
                .Subscribe(x =>
                {
                    this.RegisterActorEvent(x);
                })
                .AddTo(this);

            GameEvent.OnSpawnedGameCameraControllerSubject()
                .Subscribe(x =>
                {
                    this.worldCamera = x.ControlledCamera;
                })
                .AddTo(this);
        }

        private void RegisterActorEvent(Actor actor)
        {
            actor.Event.OnTakedDamageSubject()
                .Subscribe(x =>
                {
                    var element = this.damageLabelUIView.CreateElement(x.ToString());
                    Vector2 position;
                    var screenPosition = RectTransformUtility.WorldToScreenPoint(this.worldCamera, actor.transform.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasTransform, screenPosition, this.uiCamera, out position);
                    element.transform.localPosition = position;
                })
                .AddTo(this);
        }
    }
}
