using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System.Linq;
using ER.StageControllers;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

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

        private List<Vector3Int> stageIndexies = null;

        private Vector2 buttonScrollView;

        private Vector3Int editingIndex;

        private StageChunk editingStageChunk;

        [MenuItem("Window/ER/StageEditor")]
        private static void OpenWindow()
        {
            GetWindow<StageEditorWindow>();
        }

        private void OnGUI()
        {
            if (this.stageIndexies == null)
            {
                this.CalculateStageIndexies();
            }

            var width = (Mathf.Abs(this.min.x) + Mathf.Abs(this.max.x) + 2) * ButtonSize;
            var height = (Mathf.Abs(this.min.y) + Mathf.Abs(this.max.y) + 2) * ButtonSize;
            using (var scope = new GUI.ScrollViewScope(new Rect(0, 0, 400, this.position.height), this.buttonScrollView, new Rect(this.min.x * ButtonSize, -this.max.y * ButtonSize, width, height), true, true))
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
            var defaultColor = GUI.color;
            foreach (var i in this.stageIndexies)
            {
                var index = i;
                GUI.color = (this.editingStageChunk != null && this.editingIndex == index) ? Color.green : defaultColor;
                if (GUI.Button(new Rect(i.x * ButtonSize + 20, -i.y * ButtonSize + 20, ButtonSize, ButtonSize), $"({index.x},{index.y})"))
                {
                    this.editingStageChunk = AssetDatabase.LoadAssetAtPath<StageChunk>($"Assets/Prefabs/Stage.Chunk({index.x},{index.y},{index.z}).prefab");
                    this.editingIndex = index;
                    AssetDatabase.OpenAsset(this.editingStageChunk.gameObject.GetInstanceID());
                }
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

            if (GUILayout.Button("Build Around Chunks"))
            {
                for (var y = -1; y <= 1; y++)
                {
                    for (var x = -1; x <= 1; x++)
                    {
                        var index = new Vector3Int(this.editingIndex.x + x, this.editingIndex.y + y, this.editingIndex.z);
                        CreateStageChunk(index);
                    }
                }

                this.CalculateStageIndexies();
            }

            if (GUILayout.Button("Remove"))
            {
                if (EditorUtility.DisplayDialog("確認", "本当に削除しますか？", "OK", "CANCEL"))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this.editingStageChunk));
                    this.CalculateStageIndexies();
                }
            }
            
            EditorGUILayout.LabelField("Add To...");
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Left"))
                {
                    this.CreateStageChunk(new Vector3Int(this.editingIndex.x -1, this.editingIndex.y, this.editingIndex.z));
                    this.CalculateStageIndexies();
                }
                if (GUILayout.Button("Top"))
                {
                    this.CreateStageChunk(new Vector3Int(this.editingIndex.x, this.editingIndex.y + 1, this.editingIndex.z));
                    this.CalculateStageIndexies();
                }
                if (GUILayout.Button("Right"))
                {
                    this.CreateStageChunk(new Vector3Int(this.editingIndex.x + 1, this.editingIndex.y, this.editingIndex.z));
                    this.CalculateStageIndexies();
                }
                if (GUILayout.Button("Bottom"))
                {
                    this.CreateStageChunk(new Vector3Int(this.editingIndex.x, this.editingIndex.y - 1, this.editingIndex.z));
                    this.CalculateStageIndexies();
                }
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
                    if (i.name == "DontDestroyObject")
                    {
                        continue;
                    }
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
            if (GUILayout.Button("Refresh"))
            {
                this.CalculateStageIndexies();
            }
        }

        private void CreateStageChunk(Vector3Int index)
        {
            var path = $"Assets/Prefabs/Stage.Chunk({index.x},{index.y},{index.z}).prefab";
            if (AssetDatabase.LoadAssetAtPath<StageChunk>(path) == null)
            {
                var basePath = "Assets/Prefabs/Stage.Chunk.Base.prefab";
                var basePrefab = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(basePath));
                PrefabUtility.SaveAsPrefabAssetAndConnect(basePrefab, path, InteractionMode.AutomatedAction);
                DestroyImmediate(basePrefab);
                Debug.Log(index);
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

        private void CalculateStageIndexies()
        {
            this.stageIndexies = AssetDatabase
                .FindAssets("Stage.Chunk(", new string[] { "Assets/Prefabs" })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => GetStageIndex(path))
                .ToList();

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

            foreach (var i in this.stageIndexies)
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
    }
}
