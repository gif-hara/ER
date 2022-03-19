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
    [CreateAssetMenu(menuName = "ER/MasterData/Armor")]
    public sealed class MasterDataArmor : MasterData<MasterDataArmor, MasterDataArmor.Record>
    {
        [Serializable]
        public class Record : IIdHolder<string>, IEquipmentData
        {
            [SerializeField, TermsPopup]
            private string id = default;

            [SerializeField]
            private int physicsDefense = default;

            [SerializeField]
            private int magicDefense = default;

            [SerializeField]
            private int fireDefense = default;

            [SerializeField]
            private int earthDefense = default;

            [SerializeField]
            private int thunderDefense = default;

            [SerializeField]
            private int waterDefense = default;

            [SerializeField]
            private int holyDefense = default;

            [SerializeField]
            private int darkDefense = default;

            public int GetDefense(AttackAttributeType attackAttributeType)
            {
                switch (attackAttributeType)
                {
                    case AttackAttributeType.Physics:
                        return this.physicsDefense;
                    case AttackAttributeType.Magic:
                        return this.magicDefense;
                    case AttackAttributeType.Fire:
                        return this.fireDefense;
                    case AttackAttributeType.Earth:
                        return this.earthDefense;
                    case AttackAttributeType.Thunder:
                        return this.thunderDefense;
                    case AttackAttributeType.Water:
                        return this.waterDefense;
                    case AttackAttributeType.Holy:
                        return this.holyDefense;
                    case AttackAttributeType.Dark:
                        return this.darkDefense;
                    default:
                        Assert.IsTrue(false, $"{attackAttributeType}は未対応です");
                        return 0;
                }
            }

            public string Id => this.id;

            public string Name => LocalizationManager.GetTermTranslation(this.id);

            public Record(
                string id,
                int physicsDefense,
                int magicDefense,
                int fireDefense,
                int earthDefense,
                int thunderDefense,
                int waterDefense,
                int holyDefense,
                int darkDefense
                )
            {
                this.id = id;
                this.physicsDefense = physicsDefense;
                this.magicDefense = magicDefense;
                this.fireDefense = fireDefense;
                this.earthDefense = earthDefense;
                this.thunderDefense = thunderDefense;
                this.waterDefense = waterDefense;
                this.holyDefense = holyDefense;
                this.darkDefense = darkDefense;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Download")]
        private async void Download()
        {
            var task = DownloadFromSpreadSheet("Armor");
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
            public string PhysicsDefense;
            public string MagicDefense;
            public string FireDefense;
            public string EarthDefense;
            public string ThunderDefense;
            public string WaterDefense;
            public string HolyDefense;
            public string DarkDefense;

            public Record ToRecord() => new Record(
                $"Item/{this.Id}",
                int.Parse(this.PhysicsDefense),
                int.Parse(this.MagicDefense),
                int.Parse(this.FireDefense),
                int.Parse(this.EarthDefense),
                int.Parse(this.ThunderDefense),
                int.Parse(this.WaterDefense),
                int.Parse(this.HolyDefense),
                int.Parse(this.DarkDefense)
                );
        }
#endif
    }
}
