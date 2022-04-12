using System.Collections.Generic;
using UniRx;
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

        public void Invoke(IBehaviourData data, CompositeDisposable disposable)
        {
            foreach (var behaviour in this.behaviours)
            {
                behaviour.Invoke(data, disposable);
            }
        }
    }
}
