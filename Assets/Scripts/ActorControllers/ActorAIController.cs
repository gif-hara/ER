using ER.EquipmentSystems;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorAIController : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;

        [SerializeField]
        private ActorAIElement initialAI = default;

        [SerializeReference, SubclassSelector(typeof(IEquipmentSelector))]
        private IEquipmentSelector rightEquipmentSelector = default;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        private ActorAIElement nextAI;

        private ActorAIElement currentAI;

        private ActorAIBehaviourData behaviourData;

        private void Start()
        {
            this.ChangeRequest(this.initialAI);
            this.rightEquipmentSelector.Attach(this.actor, 0);

            this.behaviourData = new ActorAIBehaviourData
            {
                Actor = this.actor,
                AIController = this
            };
        }

        private void Update()
        {
            this.ChangeInternal();

            foreach (var i in this.currentAI.Behaviours)
            {
                i.Invoke(this.behaviourData, this.disposables);
            }
        }

        public void ChangeRequest(ActorAIElement nextAI)
        {
            this.nextAI = nextAI;
            Assert.IsNotNull(this.nextAI);
        }

        private void ChangeInternal()
        {
            if (this.nextAI == null)
            {
                return;
            }

            this.disposables.Clear();

            this.currentAI = this.nextAI;
            this.nextAI = null;
        }

        [Serializable]
        public class AIElement
        {
            public string aiName = default;

            public List<ERBehaviour.Behaviour> behaviours = default;
        }
    }
}
