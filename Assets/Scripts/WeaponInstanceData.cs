using ER.MasterDataSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WeaponInstanceData : IEquipmentData
    {
        public MasterDataWeapon.Record MasterData { get; }

        public WeaponLevelData LevelData { get; }

        public WeaponInstanceData(MasterDataWeapon.Record masterData, WeaponLevelData levelData)
        {
            this.MasterData = masterData;
            this.LevelData = levelData;
        }

        public int GetAttack(AttackAttributeType attackAttributeType)
        {
            return this.MasterData.GetAttackElement(attackAttributeType).Evaluate(this.LevelData.GetRate(attackAttributeType));
        }
    }
}
