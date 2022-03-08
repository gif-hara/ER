using ER.EquipmentSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquipmentControllerHolder
    {
        EquipmentController EquipmentController { get; }
    }
}
