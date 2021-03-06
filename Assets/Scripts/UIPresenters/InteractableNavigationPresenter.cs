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
    public sealed class InteractableNavigationPresenter : UIPresenter
    {
        [SerializeField]
        private InteractableNavigationUIView interactableNavigationUIView = default;

        private void Awake()
        {
            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
            .Where(x => x.SpawnedActor.gameObject.layer == Layer.Index.Player)
            .Subscribe(x =>
            {
                this.RegisterActorEvent(x.SpawnedActor);
                this.interactableNavigationUIView.AnimationController.PlayImmediate(false);
            })
            .AddTo(this);
        }

        private void RegisterActorEvent(Actor actor)
        {
            actor.Broker.Receive<ActorEvent.OnEnterInteractableStageGimmick>()
                .Subscribe(x =>
                {
                    this.interactableNavigationUIView.Message = x.Gimmick.LocalizedNavigationMessage;
                    this.interactableNavigationUIView.AnimationController.Play(true);
                })
                .AddTo(this);

            actor.Broker.Receive<ActorEvent.OnExitInteractableStageGimmick>()
                .Subscribe(_ =>
                {
                    this.interactableNavigationUIView.AnimationController.Play(false);
                })
                .AddTo(this);
        }
    }
}
