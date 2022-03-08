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
        public static T Cast<T>(this IBehaviourData self) where T : class
        {
            var result = self as T;
            Assert.IsNotNull(result, $"{self.GetType()}に{typeof(T)}はありません");

            return result;
        }
    }
}
