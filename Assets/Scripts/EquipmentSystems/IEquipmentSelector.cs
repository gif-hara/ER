using ER.ActorControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.EquipmentSystems
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquipmentSelector
    {
        /// <summary>
        /// <see cref="Actor"/>の右手に装備品をアタッチする
        /// </summary>
        void AttachRight(Actor actor);
    }
}
