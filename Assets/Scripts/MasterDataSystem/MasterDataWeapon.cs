using ER.EquipmentSystems;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.MasterDataSystem
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "ER/MasterData/Weapon")]
    public sealed class MasterDataWeapon : MasterData<MasterDataWeapon, MasterDataWeapon.Record>
    {
        [Serializable]
        public class Record : IIdHolder<string>
        {
            [SerializeField, TermsPopup]
            private string id = default;

            [SerializeField]
            private EquipmentController equipmentControllerPrefab = default;

            [SerializeField]
            private AttackElement physics = default;

            [SerializeField]
            private AttackElement magic = default;

            [SerializeField]
            private AttackElement fire = default;

            [SerializeField]
            private AttackElement earth = default;

            [SerializeField]
            private AttackElement thunder = default;

            [SerializeField]
            private AttackElement water = default;

            [SerializeField]
            private AttackElement holy = default;

            [SerializeField]
            private AttackElement dark = default;

            public string Id => this.id;

            public EquipmentController EquipmentControllerPrefab => this.equipmentControllerPrefab;

            public AttackElement Physics => this.physics;

            public AttackElement Magic => this.magic;

            public AttackElement Fire => this.fire;

            public AttackElement Earth => this.earth;

            public AttackElement Thunder => this.thunder;

            public AttackElement Water => this.water;

            public AttackElement Holy => this.holy;

            public AttackElement Dark => this.dark;

            public AttackElement GetAttackElement(AttackAttributeType attackAttributeType)
            {
                switch (attackAttributeType)
                {
                    case AttackAttributeType.Physics:
                        return this.physics;
                    case AttackAttributeType.Magic:
                        return this.magic;
                    case AttackAttributeType.Fire:
                        return this.fire;
                    case AttackAttributeType.Earth:
                        return this.earth;
                    case AttackAttributeType.Thunder:
                        return this.thunder;
                    case AttackAttributeType.Water:
                        return this.water;
                    case AttackAttributeType.Holy:
                        return this.holy;
                    case AttackAttributeType.Dark:
                        return this.dark;
                    default:
                        Assert.IsTrue(false, $"{attackAttributeType}は未対応です");
                        return null;
                }
            }

            public Record(
                string id,
                EquipmentController equipmentControllerPrefab,
                AttackElement physics,
                AttackElement magic,
                AttackElement fire,
                AttackElement earth,
                AttackElement thunder,
                AttackElement water,
                AttackElement holy,
                AttackElement dark
                )
            {
                this.id = id;
                this.equipmentControllerPrefab = equipmentControllerPrefab;
                this.physics = physics;
                this.magic = magic;
                this.fire = fire;
                this.earth = earth;
                this.thunder = thunder;
                this.water = water;
                this.holy = holy;
                this.dark = dark;
            }
        }

        [Serializable]
        public class AttackElement
        {
            public int minAttack = default;

            public int maxAttack = default;

            public EquipmentGrowthType growthType = default;

            public int Evalute(float value)
            {
                return Mathf.FloorToInt(Mathf.Lerp(this.minAttack, this.maxAttack, Ease.Evalute(value, this.growthType)));
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Download")]
        private async void Download()
        {
            var task = DownloadFromSpreadSheet("Weapon");
            await task;

            Debug.Log(task.Result);
            var result = JsonUtility.FromJson<Json>(task.Result);

            this.records = result.elements.Select(x => x.ToRecord()).ToList();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [Serializable]
        private class Json
        {
            public List<JsonElement> elements;
        }

        [Serializable]
        private class JsonElement
        {
            public string Id;
            public string PrefabName;
            public string PhysicsMin;
            public string PhysicsMax;
            public string PhysicsCurve;
            public string MagicMin;
            public string MagicMax;
            public string MagicCurve;
            public string FireMin;
            public string FireMax;
            public string FireCurve;
            public string EarthMin;
            public string EarthMax;
            public string EarthCurve;
            public string ThunderMin;
            public string ThunderMax;
            public string ThunderCurve;
            public string WaterMin;
            public string WaterMax;
            public string WaterCurve;
            public string HolyMin;
            public string HolyMax;
            public string HolyCurve;
            public string DarkMin;
            public string DarkMax;
            public string DarkCurve;

            public Record ToRecord() => new Record(
                $"Item/{this.Id}",
                UnityEditor.AssetDatabase.LoadAssetAtPath<EquipmentController>($"Assets/Prefabs/Equipment.Weapon.{this.PrefabName}.prefab"),
                new AttackElement
                {
                    minAttack = int.Parse(this.PhysicsMin),
                    maxAttack = int.Parse(this.PhysicsMax),
                    growthType = (EquipmentGrowthType)Enum.Parse(typeof(EquipmentGrowthType), this.PhysicsCurve)
                },
                new AttackElement
                {
                    minAttack = int.Parse(this.MagicMin),
                    maxAttack = int.Parse(this.MagicMax),
                    growthType = (EquipmentGrowthType)Enum.Parse(typeof(EquipmentGrowthType), this.MagicCurve)
                },
                new AttackElement
                {
                    minAttack = int.Parse(this.FireMin),
                    maxAttack = int.Parse(this.FireMax),
                    growthType = (EquipmentGrowthType)Enum.Parse(typeof(EquipmentGrowthType), this.FireCurve)
                },
                new AttackElement
                {
                    minAttack = int.Parse(this.EarthMin),
                    maxAttack = int.Parse(this.EarthMax),
                    growthType = (EquipmentGrowthType)Enum.Parse(typeof(EquipmentGrowthType), this.EarthCurve)
                },
                new AttackElement
                {
                    minAttack = int.Parse(this.ThunderMin),
                    maxAttack = int.Parse(this.ThunderMax),
                    growthType = (EquipmentGrowthType)Enum.Parse(typeof(EquipmentGrowthType), this.ThunderCurve)
                },
                new AttackElement
                {
                    minAttack = int.Parse(this.WaterMin),
                    maxAttack = int.Parse(this.WaterMax),
                    growthType = (EquipmentGrowthType)Enum.Parse(typeof(EquipmentGrowthType), this.WaterCurve)
                },
                new AttackElement
                {
                    minAttack = int.Parse(this.HolyMin),
                    maxAttack = int.Parse(this.HolyMax),
                    growthType = (EquipmentGrowthType)Enum.Parse(typeof(EquipmentGrowthType), this.HolyCurve)
                },
                new AttackElement
                {
                    minAttack = int.Parse(this.DarkMin),
                    maxAttack = int.Parse(this.DarkMax),
                    growthType = (EquipmentGrowthType)Enum.Parse(typeof(EquipmentGrowthType), this.DarkCurve)
                }
                );
        }
#endif
    }
}
