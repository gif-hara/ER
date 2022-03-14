using UnityEngine;
using UnityEngine.Assertions;

namespace ER.StageControllers
{
    /// <summary>
    /// ステージギミックのインターフェイス
    /// </summary>
    public interface IStageGimmick
    {
        /// <summary>
        /// ギミックを利用可能な状態にする
        /// </summary>
        void Setup(StageController stageController);
    }
}
