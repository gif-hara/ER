using System;
using System.Collections.Generic;
using ER.ERBehaviour;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class IWeightExtensions
    {
        public static T Lottery<T>(this IEnumerable<T> self) where T : IWeight
        {
            var max = 0;
            foreach (var i in self)
            {
                max += i.Weight;
            }

            var current = 0;
            var random = Random.Range(0, max);
            foreach (var i in self)
            {
                if (current <= random && random < (current + i.Weight))
                {
                    return i;
                }

                current += i.Weight;
            }
            
            Assert.IsTrue(false, $"不正な挙動です max = {max}, random = {random}");
            return default;
        }
    }
}
