using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System.Linq;
using ER.StageControllers;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace ER.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageEditorWindow : EditorWindow
    {
        private const int ButtonSize = 50;

        private Vector3Int min;

        private Vector3Int max;

        private Vector2 buttonScrollView;

        private Vector2Int editingIndex;

        private StageChunk editingStageChunk;

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
        }

        private void OnGUI()
        {
            var xMax = Mathf.Abs(this.min.x) + Mathf.Abs(this.max.x);
            var yMax = Mathf.Abs(this.min.y) + Mathf.Abs(this.max.y);
            using (var scope = new GUI.ScrollViewScope(new Rect(0, 0, 400, this.position.height), this.buttonScrollView, new Rect(0, 0, xMax * ButtonSize + 40, yMax * ButtonSize + 120), true, true))
            {
                this.DrawButtons();

                this.buttonScrollView = scope.scrollPosition;
            }
            using (new GUILayout.AreaScope(new Rect(400, 0, this.position.width - 400, this.position.height / 2)))
            {
                this.DrawPrefabInformation();
            }
            using (new GUILayout.AreaScope(new Rect(400, this.position.height / 2, this.position.width - 400, this.position.height / 2)))
            {
                this.DrawSystem();
            }
        }

        private void DrawButtons()
        {
            var xPosition = 0;
            var yPosition = 0;
            var defaultColor = GUI.color;
            for (var y = this.max.y; y >= this.min.y; y--)
            {
                for (var x = this.min.x; x <= this.max.x; x++)
                {
                    var index = new Vector2Int(x, y);
                    GUI.color = (this.editingStageChunk != null && this.editingIndex == index) ? Color.green : defaultColor;
                    if (GUI.Button(new Rect(xPosition * ButtonSize + 20, yPosition * ButtonSize + 20, ButtonSize, ButtonSize), $"({x},{y})"))
                    {
                        this.editingStageChunk = AssetDatabase.LoadAssetAtPath<StageChunk>($"Assets/Prefabs/Stage.Chunk({x},{y},0).prefab");
                        this.editingIndex = index;
                        AssetDatabase.OpenAsset(this.editingStageChunk.gameObject.GetInstanceID());
                    }
                    xPosition++;
                }

                xPosition = 0;
                yPosition++;
            }

            GUI.color = defaultColor;
        }

        private void DrawPrefabInformation()
        {
            if (this.editingStageChunk == null)
            {
                GUILayout.Label("ステージを選択してください");
                return;
            }

            EditorGUILayout.LabelField("Prefab Informaiton");
            EditorGUI.indentLevel++;

            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.ObjectField("Path", this.editingStageChunk, typeof(StageChunk), false);
            }
            EditorGUI.indentLevel--;
        }

        private void DrawSystem()
        {
            EditorGUILayout.LabelField("System");
            if (GUILayout.Button("Open All"))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/EditStage.unity");
                foreach (var i in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    DestroyImmediate(i);
                }

                var chunks = AssetDatabase
                    .FindAssets("Stage.Chunk(", new string[] { "Assets/Prefabs" })
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .Select(path => AssetDatabase.LoadAssetAtPath<StageChunk>(path));

                foreach (var i in chunks)
                {
                    var stageChunk = (StageChunk)PrefabUtility.InstantiatePrefab(i);
                    stageChunk.SetPositionFromId();
                }
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
