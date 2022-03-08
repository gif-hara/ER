using ER.ERBehaviour;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.EquipmentSystems
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class EquipmentData
    {
        public float coolTime;

        public List<ERBehaviour.Behaviour> behaviours;
    }
}
