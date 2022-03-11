using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class ActorStatusData
    {
        /// <summary>
        /// 体力
        /// </summary>
        public int physical = default;

        /// <summary>
        /// 筋力
        /// </summary>
        public int strength = default;

        /// <summary>
        /// 魔力
        /// </summary>
        public int magic = default;

        /// <summary>
        /// 忍耐力
        /// </summary>
        public int endurance = default;

        /// <summary>
        /// 精神力
        /// </summary>
        public int spirit = default;

        public int HitPoint => this.physical * 3;
    }
}
