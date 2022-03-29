using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System.Linq;

namespace ER.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageEditorWindow : EditorWindow
    {
        private const int ButtonSize = 30;

        private const int Padding = 10;

        private Vector3Int min;

        private Vector3Int max;

        private float rowPadding;

        [MenuItem("Window/ER/StageEditor")]
        private static void OpenWindow()
        {
            GetWindow<StageEditorWindow>();
        }

        private void OnEnable()
        {
            var indexies = AssetDatabase
                .FindAssets("Stage.Chunk(", new string[] { "Assets/Prefabs" })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => GetStageIndex(path));

            this.min = new Vector3Int(
                int.MaxValue,
                int.MaxValue,
                int.MaxValue
                );

            this.max = new Vector3Int(
                int.MinValue,
                int.MinValue,
                int.MinValue
                );

            foreach (var i in indexies)
            {
                if (this.min.x >= i.x)
                {
                    this.min.x = i.x;
                }
                if (this.max.x <= i.x)
                {
                    this.max.x = i.x;
                }
                if (this.min.y >= i.y)
                {
                    this.min.y = i.y;
                }
                if (this.max.y <= i.y)
                {
                    this.max.y = i.y;
                }
                if (this.min.z >= i.z)
                {
                    this.min.z = i.z;
                }
                if (this.max.z <= i.z)
                {
                    this.max.z = i.z;
                }
            }

            Debug.Log($"min = {this.min}, max = {this.max}");
        }

        private void OnGUI()
        {
            this.DrawRowNumber();
            this.DrawColumnNumber();
            this.DrawButtons();
        }

        private void DrawRowNumber()
        {
            var count = 0;
            for (var i = this.min.y; i <= this.max.y; i++)
            {
                GUI.Label(new Rect(10, count * 30 + 20, ButtonSize, ButtonSize), i.ToString());
                count++;
            }
        }

        private void DrawColumnNumber()
        {
            var count = 0;
            for (var i = this.min.x; i <= this.max.x; i++)
            {
                GUI.Label(new Rect(count * 30 + 40, -5, ButtonSize, ButtonSize), i.ToString());
                count++;
            }
        }

        private void DrawButtons()
        {
            var xPosition = 0;
            var yPosition = 0;
            for (var y = this.min.y; y <= this.max.y; y++)
            {
                for (var x = this.min.x; x <= this.max.x; x++)
                {
                    if (GUI.Button(new Rect(xPosition * ButtonSize + 30, yPosition * ButtonSize + 20, ButtonSize, ButtonSize), ""))
                    {
                        Debug.Log($"x = {x}, y = {y}");
                    }
                    xPosition++;
                }

                xPosition = 0;
                yPosition++;
            }
        }

        private Vector3Int GetStageIndex(string name)
        {
            var startIndex = name.IndexOf("(");
            if (startIndex == -1 && name.IndexOf("Base") == -1)
            {
                Debug.Log($"{name}を正しくパースできません");
                return Vector3Int.zero;
            }

            var endIndex = name.IndexOf(",", startIndex);
            var x = int.Parse(name.Substring(startIndex + 1, endIndex - startIndex - 1));
            startIndex = endIndex;
            endIndex = name.IndexOf(",", startIndex + 1);
            var y = int.Parse(name.Substring(startIndex + 1, endIndex - startIndex - 1));
            startIndex = endIndex;
            endIndex = name.IndexOf(")", startIndex + 1);
            var z = int.Parse(name.Substring(startIndex + 1, endIndex - startIndex - 1));

            return new Vector3Int(x, y, z);
        }
    }
}
