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
        /// <see cref="Actor"/>に装備品をアタッチする
        /// </summary>
        void Attach(Actor actor);
    }
}
