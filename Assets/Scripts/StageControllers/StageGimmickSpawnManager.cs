using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.StageControllers
{
    /// <summary>
    /// ステージ内のギミックを生成したか管理するクラス
    /// </summary>
    public sealed class StageGimmickSpawnManager
    {
        private readonly HashSet<string> spawnedEnemyIds = new HashSet<string>();

        private readonly StringBuilder stringBuilder = new StringBuilder();

        public bool CanSpawnEnemy(Transform spawner, out string id)
        {
            id = GetId(spawner);

            return !this.spawnedEnemyIds.Contains(id);
        }

        public void AddSpawnedEnemy(string id)
        {
            this.spawnedEnemyIds.Add(id);
        }

        private string GetId(Transform t)
        {
            this.stringBuilder.Clear();
            do
            {
                this.stringBuilder.Append(t.name);
                t = t.parent;
            } while (t != null);

            return this.stringBuilder.ToString();
        }
    }
}
