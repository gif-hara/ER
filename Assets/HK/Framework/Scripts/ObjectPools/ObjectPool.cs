using UnityEngine;
using UnityEngine.Assertions;

namespace HK.Framework
{
    /// <summary>
    /// オブジェクトプール
    /// </summary>
    public class ObjectPool<T> : UniRx.Toolkit.ObjectPool<T>
        where T : Component
    {
        private readonly T original;
        
        public ObjectPool(T original)
        {
            Assert.IsNotNull(original, string.Format("originalがNullでした T = {0}", typeof(T).Name));
            this.original = original;
        }

        protected override T CreateInstance()
        {
            return Object.Instantiate(this.original);
        }
    }
}
