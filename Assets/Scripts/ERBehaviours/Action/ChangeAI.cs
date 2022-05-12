using ER.ActorControllers;
using UnityEngine;

namespace ER.ERBehaviour
{
    /// <summary>
    /// 指定されたAIに切り替える
    /// </summary>
    public sealed class ChangeAI : IAction
    {
        [SerializeField]
        private BehaviourHolder nextAI = default;

        public void Invoke(IBehaviourData data)
        {
            var aiBehaviourData = data.Cast<ActorAIBehaviourData>();
            aiBehaviourData.AIController.ChangeRequest(this.nextAI);
        }
    }
}
