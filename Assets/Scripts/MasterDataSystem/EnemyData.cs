using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ER.MasterDataSystem
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "ER/MasterData/EnemyData")]
    public sealed class EnemyData : MasterData<EnemyData, EnemyData.Record>
    {
        [Serializable]
        public class Record : IIdHolder<string>
        {
            [SerializeField]
            private string id = default;

            [SerializeField, TermsPopup]
            private string name = default;

            [SerializeField]
            private int physicsAttack = default;

            [SerializeField]
            private int magicAttack = default;

            [SerializeField]
            private int fireAttack = default;

            [SerializeField]
            private int earthAttack = default;

            [SerializeField]
            private int thunderAttack = default;

            [SerializeField]
            private int waterAttack = default;

            [SerializeField]
            private int holyAttack = default;

            [SerializeField]
            private int darkAttack = default;

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

            public string Name => LocalizationManager.GetTermTranslation(this.name);

            public int PhysicsAttack => this.physicsAttack;

            public int MagicAttack => this.magicAttack;

            public int FireAttack => this.fireAttack;

            public int EarthAttack => this.earthAttack;

            public int ThunderAttack => this.thunderAttack;

            public int WaterAttack => this.waterAttack;

            public int HolyAttack => this.holyAttack;

            public int DarkAttack => this.darkAttack;

            public int PhysicsDefense => this.physicsDefense;

            public int MagicDefense => this.magicDefense;

            public int FireDefense => this.fireDefense;

            public int EarthDefense => this.earthDefense;

            public int ThunderDefense => this.thunderDefense;

            public int WaterDefense => this.waterDefense;

            public int HolyDefense => this.holyDefense;

            public int DarkDefense => this.darkDefense;

            public float PhysicsCutRate => this.physicsCutRate;

            public float MagicCutRate => this.magicCutRate;

            public float FireCutRate => this.fireCutRate;

            public float EarthCutRate => this.earthCutRate;

            public float ThunderCutRate => this.thunderCutRate;

            public float WaterCutRate => this.waterCutRate;

            public float HolyCutRate => this.holyCutRate;

            public float DarkCutRate => this.darkCutRate;

            public Record(
                string id,
                string name,
                int physicsAttack,
                int magicAttack,
                int fireAttack,
                int earthAttack,
                int thunderAttack,
                int waterAttack,
                int holyAttack,
                int darkAttack,
                int physicsDefense,
                int magicDefense,
                int fireDefense,
                int earthDefense,
                int thunderDefense,
                int waterDefense,
                int holyDefense,
                int darkDefense,
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
                this.name = name;
                this.physicsAttack = physicsAttack;
                this.magicAttack = magicAttack;
                this.fireAttack = fireAttack;
                this.earthAttack = earthAttack;
                this.thunderAttack = thunderAttack;
                this.waterAttack = waterAttack;
                this.holyAttack = holyAttack;
                this.darkAttack = darkAttack;
                this.physicsDefense = physicsDefense;
                this.magicDefense = magicDefense;
                this.fireDefense = fireDefense;
                this.earthDefense = earthDefense;
                this.thunderDefense = thunderDefense;
                this.waterDefense = waterDefense;
                this.holyDefense = holyDefense;
                this.darkDefense = darkDefense;
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
            var task = DownloadFromSpreadSheet(nameof(EnemyData));
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
            public string Name;
            public string PhysicsAttack;
            public string MagicAttack;
            public string FireAttack;
            public string EarthAttack;
            public string ThunderAttack;
            public string WaterAttack;
            public string HolyAttack;
            public string DarkAttack;
            public string PhysicsDefense;
            public string MagicDefense;
            public string FireDefense;
            public string EarthDefense;
            public string ThunderDefense;
            public string WaterDefense;
            public string HolyDefense;
            public string DarkDefense;
            public string PhysicsCutRate;
            public string MagicCutRate;
            public string FireCutRate;
            public string EarthCutRate;
            public string ThunderCutRate;
            public string WaterCutRate;
            public string HolyCutRate;
            public string DarkCutRate;

            public Record ToRecord() => new Record(
                this.Id,
                $"Enemy/{this.Name}",
                int.Parse(this.PhysicsAttack),
                int.Parse(this.MagicAttack),
                int.Parse(this.FireAttack),
                int.Parse(this.EarthAttack),
                int.Parse(this.ThunderAttack),
                int.Parse(this.WaterAttack),
                int.Parse(this.HolyAttack),
                int.Parse(this.DarkAttack),
                int.Parse(this.PhysicsDefense),
                int.Parse(this.MagicDefense),
                int.Parse(this.FireDefense),
                int.Parse(this.EarthDefense),
                int.Parse(this.ThunderDefense),
                int.Parse(this.WaterDefense),
                int.Parse(this.HolyDefense),
                int.Parse(this.DarkDefense),
                float.Parse(this.PhysicsCutRate),
                float.Parse(this.MagicCutRate),
                float.Parse(this.FireCutRate),
                float.Parse(this.EarthCutRate),
                float.Parse(this.ThunderCutRate),
                float.Parse(this.WaterCutRate),
                float.Parse(this.HolyCutRate),
                float.Parse(this.DarkCutRate)
                );
        }
#endif
    }
}
