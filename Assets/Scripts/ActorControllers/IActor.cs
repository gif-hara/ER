using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActor
    {
        /// <summary>
        /// 右手装備品の使用を開始した際のイベント
        /// </summary>
        IObservable<Unit> OnBeginRightEquipmentAsObservable();

        /// <summary>
        /// 右手装備品の使用を完了した際のイベント
        /// </summary>
        IObservable<Unit> OnEndRightEquipmentAsObservable();

        Transform transform { get; }

        GameObject gameObject { get; }

        Animator Animator { get; }

        /// <summary>
        /// 弾が衝突した際の処理
        /// </summary>
        void OnCollisionBullet(BulletController bulletController);
    }
}
