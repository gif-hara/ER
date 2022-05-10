using ER.ActorControllers;
using I2.Loc;
using System;
using System.Collections.Generic;
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
        [SerializeField]
        private List<Element> elements = default;

        private Subject<Unit> onAddedItemSubject = new Subject<Unit>();

        public IObservable<Unit> OnAddedItemAsObservable() => this.onAddedItemSubject;

        public void Setup(List<Element> elements)
        {
            this.elements = elements;
        }

        public override void BeginInteract(Actor actor)
        {
            foreach (var i in this.elements)
            {
                actor.InventoryController.AddItem(i.itemId, i.number, true);
            }
            this.onAddedItemSubject.OnNext(Unit.Default);
            Destroy(this.gameObject);
        }

        public override string LocalizedNavigationMessage => ScriptLocalization.Common.PickUp;

        [Serializable]
        public class Element
        {
            [SerializeField, TermsPopup]
            public string itemId = default;

            [SerializeField]
            public int number = 1;
        }
    }
}
