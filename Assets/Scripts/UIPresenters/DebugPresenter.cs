using ER.ActorControllers;
using ER.UIViews;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.UIPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DebugPresenter : UIPresenter
    {
        [SerializeField]
        private DebugUIView debugUIView = default;

        private StringBuilder stringBuilder = new StringBuilder();

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
            actor.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    this.stringBuilder.Clear();

                    this.stringBuilder.Append("advancedEntry = ");
                    this.stringBuilder.Append(actor.AnimationParameter.advancedEntry.ToString());
                    this.stringBuilder.AppendLine();

                    this.stringBuilder.Append("moveSpeedRate = ");
                    this.stringBuilder.Append(actor.AnimationParameter.moveSpeedRate.ToString());
                    this.stringBuilder.AppendLine();

                    this.stringBuilder.Append("invisible = ");
                    this.stringBuilder.Append(actor.AnimationParameter.invisible.ToString());
                    this.stringBuilder.AppendLine();

                    this.stringBuilder.Append("State = ");
                    this.stringBuilder.Append(actor.StateController.CurrentState.ToString());
                    this.stringBuilder.AppendLine();

                    this.stringBuilder.Append("IsLeftRequest = ");
                    this.stringBuilder.Append(actor.EquipmentController.IsLeftRequest);
                    this.stringBuilder.AppendLine();

                    this.stringBuilder.Append("Clip = ");
                    this.stringBuilder.Append(actor.AnimationController.OverrideController["Clip"].name);
                    this.stringBuilder.AppendLine();

                    this.debugUIView.Text.SetText(this.stringBuilder.ToString());
                });
        }
    }
}
