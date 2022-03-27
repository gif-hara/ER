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
    [CreateAssetMenu(menuName = "ER/MasterData/Item")]
    public sealed class MasterDataItem : MasterData<MasterDataItem, MasterDataItem.Record>
    {
        protected override void OnSetupped()
        {
            base.OnSetupped();

            // このマスターデータのみ、動的にレコードを生成しています
            // なので参照するマスターデータは先に読み込まれている必要があります

            this.raw.Clear();
            foreach (var i in MasterDataWeapon.Instance.Records)
            {
                this.raw.Add(i.Id, new Record(i.Id, false, ItemCategory.Weapon));
            }
            foreach (var i in MasterDataShield.Instance.Records)
            {
                this.raw.Add(i.Id, new Record(i.Id, false, ItemCategory.Shield));
            }
            foreach (var i in MasterDataArmor.Instance.Records)
            {
                this.raw.Add(i.Id, new Record(i.Id, false, i.ArmorType.ToItemCategory()));
            }
        }
        [Serializable]
        public class Record : IIdHolder<string>
        {
            [SerializeField, TermsPopup]
            private string id = default;

            /// <summary>
            /// スタック可能か
            /// </summary>
            [SerializeField]
            private bool stackable = default;

            [SerializeField]
            private ItemCategory category = default;

            public string Id => this.id;

            public string LocalizedName => LocalizationManager.GetTermTranslation(this.id);

            public bool Stackable => this.stackable;

            public ItemCategory Category => this.category;

            public MasterDataWeapon.Record ToWeapon() => MasterDataWeapon.Get(this.id);

            public MasterDataShield.Record ToShield() => MasterDataShield.Get(this.id);

            public MasterDataArmor.Record ToArmor() => MasterDataArmor.Get(this.id);

            public Record(string id, bool stackable, ItemCategory category)
            {
                this.id = id;
                this.stackable = stackable;
                this.category = category;
            }
        }
    }
}
