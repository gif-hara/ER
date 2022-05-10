using System;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class AcquiredItemNotificationPresenter : UIPresenter
    {
        [SerializeField]
        private AcquiredItemNotificationUIView acquiredItemNotificationUIView = default;

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
            actor.Broker.Receive<ActorEvent.OnAcquiredItem>()
                .Subscribe(x =>
                {
                    if (x.IsShowUI)
                    {
                        this.acquiredItemNotificationUIView.CreateElement($"{x.MasterDataItem.LocalizedName} *{x.Number}");
                    }
                })
                .AddTo(actor.Disposables)
                .AddTo(this);
        }
    }
}
