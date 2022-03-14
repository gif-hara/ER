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
        private List<AIElement> elements = default;

        [SerializeField]
        private string initialAiName = default;

        [SerializeField]
        private EquipmentController rightEquipmentPrefab = default;

        private readonly Dictionary<string, AIElement> runtimeElements = new Dictionary<string, AIElement>();

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        private string nextAiName;

        private void Start()
        {
            foreach(var i in this.elements)
            {
                this.runtimeElements.Add(i.aiName, i);
            }

            this.ChangeRequest(this.initialAiName);
            this.actor.EquipmentController.SetRightEquipment(this.rightEquipmentPrefab);
        }

        private void Update()
        {
            this.ChangeInternal();
        }

        public void ChangeRequest(string aiName)
        {
            this.nextAiName = aiName;
        }

        private void ChangeInternal()
        {
            if(string.IsNullOrEmpty(this.nextAiName))
            {
                return;
            }

            this.disposables.Clear();
            Assert.IsTrue(this.runtimeElements.ContainsKey(this.nextAiName), $"{this.nextAiName}というAIはありません");

            var behaviourData = new ActorAIBehaviourData
            {
                Actor = this.actor,
                AIController = this
            };
            var element = this.runtimeElements[this.nextAiName];
            foreach(var i in element.behaviours)
            {
                i.AsObservable(behaviourData)
                    .Subscribe()
                    .AddTo(this.disposables);
            }
            this.nextAiName = "";
        }

        [Serializable]
        public class AIElement
        {
            public string aiName = default;

            public List<ERBehaviour.Behaviour> behaviours = default;
        }
    }
}
