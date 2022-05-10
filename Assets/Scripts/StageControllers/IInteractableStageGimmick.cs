using UnityEngine;
using UnityEngine.Assertions;
using ER.ActorControllers;

namespace ER.StageControllers
{
    /// <summary>
    /// <see cref="Actor"/>と対話可能なステージギミックのインターフェイス
    /// </summary>
    public interface IInteractableStageGimmick : IStageGimmick
    {
        /// <summary>
        /// 対話を開始する
        /// </summary>
        void BeginInteract(Actor actor);
        
        /// <summary>
        /// ナビゲーション用のローカライズ済みメッセージを返す
        /// </summary>
        string LocalizedNavigationMessage { get; }
    }
}
