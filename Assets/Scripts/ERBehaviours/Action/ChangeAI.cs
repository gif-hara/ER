using ER.ActorControllers;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChangeAI : IAction
    {
        [SerializeField]
        private string aiName = default;

        public void Invoke(IBehaviourData data)
        {
            var aiBehaviourData = data.Cast<ActorAIBehaviourData>();
            aiBehaviourData.AIController.ChangeRequest(this.aiName);
        }
    }
}
