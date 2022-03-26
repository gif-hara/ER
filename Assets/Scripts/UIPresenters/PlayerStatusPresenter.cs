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
            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
                .Where(x => x.SpawnedActor.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.RegisterActorEvent(x.SpawnedActor);
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
                .AddTo(actor.Disposables)
                .AddTo(this);

            actor.Broker.Receive<ActorEvent.OnChangedHandEquipment>()
                .Subscribe(x =>
                {
                    if (x.HandType == HandType.Left)
                    {
                        this.playerStatusUIView.LeftHandStatus.EquipmentName.text = x.EquipmentController.Item.MasterDataItem.LocalizedName;
                    }
                    else if (x.HandType == HandType.Right)
                    {
                        this.playerStatusUIView.RightHandStatus.EquipmentName.text = x.EquipmentController.Item.MasterDataItem.LocalizedName;
                    }
                })
                .AddTo(actor.Disposables)
                .AddTo(this);

            if (actor.EquipmentController.LeftHand.CurrentEquipmentController != null)
            {
                this.playerStatusUIView.LeftHandStatus.EquipmentName.text = actor.EquipmentController.LeftHand.CurrentEquipmentController.Item.MasterDataItem.LocalizedName;
            }
            if (actor.EquipmentController.RightHand.CurrentEquipmentController != null)
            {
                this.playerStatusUIView.RightHandStatus.EquipmentName.text = actor.EquipmentController.RightHand.CurrentEquipmentController.Item.MasterDataItem.LocalizedName;
            }
        }
    }
}
