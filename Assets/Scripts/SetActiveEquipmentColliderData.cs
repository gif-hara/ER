using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// <see cref="ActorAnimationMediator.SetActiveAttackingEquipmentColliderObject"/>の引数データを持つ<see cref="ScriptableObject"/>
    /// </summary>
    [CreateAssetMenu(menuName = "ER/SetActiveEquipmentColliderData")]
    public sealed class SetActiveEquipmentColliderData : ScriptableObject
    {
        /// <summary>
        /// どのハンドタイプを設定するか
        /// </summary>
        public HandType handType;
        
        /// <summary>
        /// 有効にするか
        /// </summary>
        public bool isActive;

        /// <summary>
        /// ダメージ係数
        /// </summary>
        public float power = 1.0f;

        /// <summary>
        /// ノックバック蓄積値の係数
        /// </summary>
        public float knockBackAccumulate = 1.0f;
    }
}
