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

        public IObservable<Unit> AsObservable(IBehaviourData data)
        {
            return Observable.Defer(() =>
            {
                var aiBehaviourData = data.Cast<ActorAIBehaviourData>();
                aiBehaviourData.AIController.ChangeRequest(this.aiName);

                return Observable.ReturnUnit();
            });
        }
    }
}
