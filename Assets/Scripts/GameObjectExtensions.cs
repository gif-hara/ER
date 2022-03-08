using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// <see cref="GameObject"/>に関する拡張関数
    /// </summary>
    public static class GameObjectExtensions
    {
        public static void SetLayerRecursive(this GameObject self, int layer)
        {
            SetLayerRecursiveInternal(self.transform, layer);
        }

        private static void SetLayerRecursiveInternal(Transform target, int layer)
        {
            target.gameObject.layer = layer;
            for(var i=0; i<target.childCount; i++)
            {
                SetLayerRecursiveInternal(target.GetChild(i), layer);
            }
        }
    }
}
