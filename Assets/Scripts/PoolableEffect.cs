using HK.Framework;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PoolableEffect : MonoBehaviour
    {
        private static ObjectPoolBundle<PoolableEffect> bundle = new ObjectPoolBundle<PoolableEffect>();

        [SerializeField]
        private float destroySeconds = default;

        public PoolableEffect Rent(Vector3 position, Quaternion rotation)
        {
            var pool = bundle.Get(this);
            var instance = pool.Rent();

            instance.transform.position = position;
            instance.transform.rotation = rotation;

            Observable.Timer(TimeSpan.FromSeconds(instance.destroySeconds))
                .Subscribe(_ =>
                {
                    pool.Return(instance);
                })
                .AddTo(instance);

            return instance;
        }
    }
}
