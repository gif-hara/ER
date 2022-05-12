using System;
using System.Collections.Generic;
using ER.ActorControllers;
using UnityEngine;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 重みによるランダムなAIに切り替える
    /// </summary>
    public sealed class ChangeAIFromWeight : IAction
    {
        [SerializeField]
        private List<NextAIInfo> nextAIInfos;
        
        [Serializable]
        public class NextAIInfo : IWeight
        {
            public BehaviourHolder nextAI;

            public int weight;
            
            public int Weight => this.weight;
        }

        public void Invoke(IBehaviourData data)
        {
            var aiBehaviourData = data.Cast<ActorAIBehaviourData>();
            var nextAIInfo = this.nextAIInfos.Lottery();
            aiBehaviourData.AIController.ChangeRequest(nextAIInfo.nextAI);
        }
    }
}
