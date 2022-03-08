using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class BehaviourDataExtensions
    {
        public static T Cast<T>(this IBehaviourData self)
        {
            var result = (T)self;

            return result;
        }
    }
}
