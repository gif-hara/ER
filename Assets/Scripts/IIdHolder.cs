using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIdHolder<T>
    {
        public T Id { get; }
    }
}
