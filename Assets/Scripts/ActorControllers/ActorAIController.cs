using ER.EquipmentSystems;
using System;
using System.Collections.Generic;
using ER.ERBehaviour;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>をAIで制御するクラス
    /// </summary>
    public sealed class ActorAIController : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;

        /// <summary>
        /// 初めに起動するAI
        /// </summary>
        [SerializeField]
        private BehaviourHolder initialAI = default;

        /// <summary>
        /// ノックバックした際に切り替わるAI
        /// </summary>
        [SerializeField]
        private BehaviourHolder onKnockBackedAI = default;

        [SerializeReference, SubclassSelector(typeof(IEquipmentSelector))]
        private IEquipmentSelector rightEquipmentSelector = default;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        private BehaviourHolder nextAI;

        private BehaviourHolder currentAI;

        private ActorAIBehaviourData behaviourData;

        private void Start()
        {
            this.actor.Broker.Receive<ActorEvent.OnChangedStateType>()
                .Buffer(2, 1)
                .Subscribe(x =>
                {
                    if (x[0].NextState == ActorStateController.StateType.KnockBack)
                    {
                        this.ChangeRequest(this.onKnockBackedAI);
                    }
                })
                .AddTo(this.actor.Disposables);
            
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

        /// <summary>
        /// <paramref name="nextAI"/>を切り替えるリクエストを行う
        /// </summary>
        public void ChangeRequest(BehaviourHolder nextAI)
        {
            this.nextAI = nextAI;
            Assert.IsNotNull(this.nextAI);
        }

        /// <summary>
        /// AIを切り替える
        /// </summary>
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
