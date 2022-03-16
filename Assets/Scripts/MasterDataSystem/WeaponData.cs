using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.MasterDataSystem
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "ER/MasterData/WeaponData")]
    public sealed class WeaponData : MasterData<WeaponData>
    {
        [SerializeField]
        private List<Record> records = default;

        public HashSet<string> IdHashSet { get; } = new HashSet<string>();

        protected override void OnSetupped()
        {
            base.OnSetupped();
            foreach(var i in this.records)
            {
                this.IdHashSet.Add(i.Id);
            }
        }

        [Serializable]
        public class Record
        {
            [SerializeField, TermsPopup]
            private string id;

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

            public AttackElement Physics => this.physics;

            public AttackElement Magic => this.magic;

            public AttackElement Fire => this.fire;

            public AttackElement Earth => this.earth;

            public AttackElement Thunder => this.thunder;

            public AttackElement Water => this.water;

            public AttackElement Holy => this.holy;

            public AttackElement Dark => this.dark;
        }

        [Serializable]
        public class AttackElement
        {
            public int minAttack = default;

            public int maxAttack = default;

            public AnimationCurve growthCurve = default;
        }
    }
}