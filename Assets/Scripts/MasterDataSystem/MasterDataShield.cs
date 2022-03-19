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
    [CreateAssetMenu(menuName = "ER/MasterData/Shield")]
    public sealed class MasterDataShield : MasterData<MasterDataShield, MasterDataShield.Record>
    {
        [Serializable]
        public class Record : IIdHolder<string>, IEquipmentData
        {
            [SerializeField, TermsPopup]
            private string id = default;

            [SerializeField]
            private EquipmentController equipmentControllerPrefab = default;

            [SerializeField]
            private float physicsCutRate = default;

            [SerializeField]
            private float magicCutRate = default;

            [SerializeField]
            private float fireCutRate = default;

            [SerializeField]
            private float earthCutRate = default;

            [SerializeField]
            private float thunderCutRate = default;

            [SerializeField]
            private float waterCutRate = default;

            [SerializeField]
            private float holyCutRate = default;

            [SerializeField]
            private float darkCutRate = default;

            public string Id => this.id;

            public string Name => LocalizationManager.GetTermTranslation(this.id);

            public EquipmentController EquipmentControllerPrefab => this.equipmentControllerPrefab;

            public float GetCutRate(AttackAttributeType attackAttributeType)
            {
                switch (attackAttributeType)
                {
                    case AttackAttributeType.Physics:
                        return this.physicsCutRate;
                    case AttackAttributeType.Magic:
                        return this.magicCutRate;
                    case AttackAttributeType.Fire:
                        return this.fireCutRate;
                    case AttackAttributeType.Earth:
                        return this.earthCutRate;
                    case AttackAttributeType.Thunder:
                        return this.thunderCutRate;
                    case AttackAttributeType.Water:
                        return this.waterCutRate;
                    case AttackAttributeType.Holy:
                        return this.holyCutRate;
                    case AttackAttributeType.Dark:
                        return this.darkCutRate;
                    default:
                        Assert.IsTrue(false, $"{attackAttributeType}は未対応です");
                        return 0.0f;
                }
            }

            public Record(
                string id,
                EquipmentController equipmentControllerPrefab,
                float physicsCutRate,
                float magicCutRate,
                float fireCutRate,
                float earthCutRate,
                float thunderCutRate,
                float waterCutRate,
                float holyCutRate,
                float darkCutRate
                )
            {
                this.id = id;
                this.equipmentControllerPrefab = equipmentControllerPrefab;
                this.physicsCutRate = physicsCutRate;
                this.magicCutRate = magicCutRate;
                this.fireCutRate = fireCutRate;
                this.earthCutRate = earthCutRate;
                this.thunderCutRate = thunderCutRate;
                this.waterCutRate = waterCutRate;
                this.holyCutRate = holyCutRate;
                this.darkCutRate = darkCutRate;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Download")]
        private async void Download()
        {
            var task = DownloadFromSpreadSheet("Shield");
            await task;

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
            public string PhysicsCutRate;
            public string MagicCutRate;
            public string FireCutRate;
            public string EarthCutRate;
            public string ThunderCutRate;
            public string WaterCutRate;
            public string HolyCutRate;
            public string DarkCutRate;

            public Record ToRecord() => new Record(
                $"Item/{this.Id}",
                UnityEditor.AssetDatabase.LoadAssetAtPath<EquipmentController>($"Assets/Prefabs/Equipment.Shield.{this.PrefabName}.prefab"),
                int.Parse(this.PhysicsCutRate),
                int.Parse(this.MagicCutRate),
                int.Parse(this.FireCutRate),
                int.Parse(this.EarthCutRate),
                int.Parse(this.ThunderCutRate),
                int.Parse(this.WaterCutRate),
                int.Parse(this.HolyCutRate),
                int.Parse(this.DarkCutRate)
                );
        }
#endif
    }
}
