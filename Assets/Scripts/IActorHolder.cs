using ER.ActorControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActorHolder
    {
        IActor Actor { get; }
    }
}
