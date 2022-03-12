using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class Layer
    {
        public static class Index
        {
            public const int Player = 6;

            public const int PlayerBullet = 7;

            public const int Enemy = 8;

            public const int EnemyBullet = 9;

            public const int Stage = 10;

            public static bool IsMatch(GameObject gameObject, int layer)
            {
                return gameObject.layer == layer;
            }
        }

        public static class Mask
        {
            public const int Player = 1 << Index.Player;

            public const int PlayerBullet = 1 << Index.PlayerBullet;

            public const int Enemy = 1 << Index.Enemy;

            public const int EnemyBullet = 1 << Index.EnemyBullet;

            public const int Stage = 1 << Index.Stage;
        }
    }
}
