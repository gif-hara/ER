using ER.ActorControllers;
using ER.UIViews;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace ER.UIPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChangeEquipmentPresenter : UIPresenter
    {
        [SerializeField]
        private ChangeEquipmentUIView changeEquipmentUIView = default;

        private Actor actor;

        private void Awake()
        {
            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
                .Where(x => x.SpawnedActor.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.actor = x.SpawnedActor;
                })
                .AddTo(this);
        }

        public void Activate()
        {
            var buttons = this.changeEquipmentUIView.GetAllButtons();
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            buttons.SetupVerticalNavigations();

            Debug.Log("?");
        }
    }
}
