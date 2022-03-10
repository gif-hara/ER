using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorStatusController
    {
        private ActorStatus baseStatus = default;

        public int HitPointMax { get; private set; }

        public int HitPoint { get; private set; }

        public void Setup(ActorStatus status)
        {
            this.baseStatus = status;
            this.HitPointMax = this.baseStatus.HitPoint;
            this.HitPoint = this.HitPointMax;
        }
    }
}
