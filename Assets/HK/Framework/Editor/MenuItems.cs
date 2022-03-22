#if UNITY_EDITOR
using HK.Framework.Systems;
using UnityEditor;

namespace HK.Framework.Editor
{
    /// <summary>
    /// MenuItemを定義するクラス
    /// </summary>
    public static class MenuItems
    {
        [MenuItem("HK/SaveData/Clear All")]
        private static void ClearAll()
        {
            if (EditorUtility.DisplayDialog("セーブデータ削除", "本当に削除しますか？", "OK", "Cancel"))
            {
                SaveData.Clear();
            }
        }

        [MenuItem("HK/SaveData/View Path")]
        private static void ViewPath()
        {
            EditorUtility.RevealInFinder(SaveData.Path);
        }
    }
}
#endif
