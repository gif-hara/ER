using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// Gameシーンのデータやり取りを行うクラス
    /// </summary>
    public sealed class GameSceneContext : ISceneContext
    {
        /// <summary>
        /// 初期姿勢のオーバーライドが有効か
        /// </summary>
        public bool canOverrideStartTransform;
        
        /// <summary>
        /// 初期座標
        /// </summary>
        public Vector3 startPosition;

        /// <summary>
        /// 初期回転値
        /// </summary>
        public Quaternion startRotation;
    }
}
