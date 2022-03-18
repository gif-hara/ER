using ER.MasterDataSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnemyManager
    {
        private readonly Dictionary<Actor, EnemyData> enemyData = new Dictionary<Actor, EnemyData>();
    }
}
