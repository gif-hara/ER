using ER.ActorControllers;
using I2.Loc;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.StageControllers
{
    /// <summary>
    /// アイテムを拾える<see cref="InteractableStageGimmick"/>
    /// </summary>
    public sealed class InteractableStageGimmickItem : InteractableStageGimmick
    {
        [SerializeField, TermsPopup]
        private string itemId = default;

        [SerializeField]
        private int number = 1;

        private Subject<Unit> onAddedItemSubject = new Subject<Unit>();

        public IObservable<Unit> OnAddedItemAsObservable() => this.onAddedItemSubject;

        public void Setup(string itemId, int number)
        {
            this.itemId = itemId;
            this.number = number;
        }

        public override void BeginInteract(Actor actor)
        {
            actor.InventoryController.AddItem(this.itemId, this.number);
            this.onAddedItemSubject.OnNext(Unit.Default);
            Destroy(this.gameObject);
        }
    }
}
