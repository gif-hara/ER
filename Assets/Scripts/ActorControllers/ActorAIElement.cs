using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorAIElement : MonoBehaviour
    {
        [SerializeField]
        private List<ERBehaviour.Behaviour> behaviours = default;

        public List<ERBehaviour.Behaviour> Behaviours => this.behaviours;
    }
}