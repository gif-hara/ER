using UnityEngine;
using UnityEngine.Assertions;

namespace ER.StageControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageChunk : MonoBehaviour
    {
#if UNITY_EDITOR
        [ContextMenu("SetPositionFromId")]
        private void SetPositionFromId()
        {
            var name = this.name;
            var startIndex = name.IndexOf("(");
            if(startIndex == -1 && name.IndexOf("Base") == -1)
            {
                Debug.Log($"{name}を正しくパースできません");
            }

            var endIndex = name.IndexOf(",", startIndex);
            var x = int.Parse(name.Substring(startIndex + 1, endIndex - startIndex - 1));
            startIndex = endIndex;
            endIndex = name.IndexOf(",", startIndex + 1);
            var y = int.Parse(name.Substring(startIndex + 1, endIndex - startIndex - 1));

            this.transform.localPosition = new Vector3(x * StageLoader.SplitSize * 2, y * StageLoader.SplitSize * 2, 0);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        public void SetupGimmicks(StageController stageController)
        {
            foreach(var i in this.GetComponentsInChildren<IStageGimmick>())
            {
                i.Setup(stageController);
            }
        }
    }
}
