using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="Actor"/>に発生するイベントを持つクラス
    /// </summary>
    public sealed class ActorEvent : IDisposable
    {
        /// <summary>
        /// 右手装備品の使用を開始した際のイベント
        /// </summary>
        public readonly Subject<Unit> beginRightEquipmentSubject = new Subject<Unit>();

        /// <summary>
        /// 右手装備品の使用を完了した際のイベント
        /// </summary>
        public readonly Subject<Unit> endRightEquipmentSubject = new Subject<Unit>();

        /// <summary>
        /// <inheritdoc cref="beginRightEquipmentSubject"/>
        /// </summary>
        public ISubject<Unit> OnBeginRightEquipmentSubject() => this.beginRightEquipmentSubject;

        /// <summary>
        /// <inheritdoc cref="endRightEquipmentSubject"/>
        /// </summary>
        public ISubject<Unit> OnEndRightEquipmentSubject() => this.endRightEquipmentSubject;

        public void Dispose()
        {
            this.beginRightEquipmentSubject.Dispose();
            this.endRightEquipmentSubject.Dispose();
        }
    }
}
