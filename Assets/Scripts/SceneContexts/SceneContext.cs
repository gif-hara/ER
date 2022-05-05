using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// シーン間のデータを置くクラス
    /// </summary>
    public static class SceneContext
    {
        private static ISceneContext current = default;

        public static void Set(ISceneContext context)
        {
            current = context;
        }

        public static T GetOrNull<T>() where T : class, ISceneContext => current as T;
    }
}
