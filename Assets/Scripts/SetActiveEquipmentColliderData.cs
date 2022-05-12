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
        public int colliderId;

        public bool isActive;
    }
}
