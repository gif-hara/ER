using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BulletBehaviourData : IBehaviourData, IGameObjectHolder
    {
        public GameObject GameObject { get; set; }
    }
}
