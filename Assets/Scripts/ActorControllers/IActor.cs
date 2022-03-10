using ER.EquipmentSystems;
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
        ActorEvent Event { get; }

        Transform transform { get; }

        GameObject gameObject { get; }

        Animator Animator { get; }

        /// <summary>
        /// 敵の攻撃が衝突した際の処理
        /// </summary>
        void OnCollisionOpponentAttack(EquipmentController equipmentController);
    }
}
