using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ERBehaviour
{
    /// <summary>
    /// <see cref="Behaviour"/>を持つだけのクラス
    /// </summary>
    public sealed class BehaviourHolder : MonoBehaviour
    {
        [SerializeField]
        private List<Behaviour> behaviours = default;

        public List<Behaviour> Behaviours => this.behaviours;
    }
}
