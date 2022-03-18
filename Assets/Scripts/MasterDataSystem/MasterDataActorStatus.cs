using ER.ActorControllers;
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
    [CreateAssetMenu(menuName = "ER/MasterData/ActorStatusData")]
    public sealed class MasterDataActorStatus : MasterData<MasterDataActorStatus, MasterDataActorStatus.Record>
    {
        [Serializable]
        public class Record : IIdHolder<string>
        {
            [SerializeField]
            private string id = default;

            public ActorStatusData statusData = default;

            public string Id => this.id;

            public Record(
                string id,
                string name,
                int hitPoint,
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
                this.statusData = new ActorStatusData()
                {
                    name = name,
                    hitPoint = hitPoint,
                    physicsAttack = physicsAttack,
                    magicAttack = magicAttack,
                    fireAttack = fireAttack,
                    earthAttack = earthAttack,
                    thunderAttack = thunderAttack,
                    waterAttack = waterAttack,
                    holyAttack = holyAttack,
                    darkAttack = darkAttack,
                    physicsDefense = physicsDefense,
                    magicDefense = magicDefense,
                    fireDefense = fireDefense,
                    earthDefense = earthDefense,
                    thunderDefense = thunderDefense,
                    waterDefense = waterDefense,
                    holyDefense = holyDefense,
                    darkDefense = darkDefense,
                    physicsCutRate = physicsCutRate,
                    magicCutRate = magicCutRate,
                    fireCutRate = fireCutRate,
                    earthCutRate = earthCutRate,
                    thunderCutRate = thunderCutRate,
                    waterCutRate = waterCutRate,
                    holyCutRate = holyCutRate,
                    darkCutRate = darkCutRate
                };
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Download")]
        private async void Download()
        {
            var task = DownloadFromSpreadSheet("ActorStatus");
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
            public string HitPoint;
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
                int.Parse(this.HitPoint),
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
