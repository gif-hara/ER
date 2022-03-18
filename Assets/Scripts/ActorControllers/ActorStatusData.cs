using I2.Loc;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class ActorStatusData
    {
        [TermsPopup]
        public string name = default;

        public int hitPoint = default;

        public int physicsAttack = default;

        public int magicAttack = default;

        public int fireAttack = default;

        public int earthAttack = default;

        public int thunderAttack = default;

        public int waterAttack = default;

        public int holyAttack = default;

        public int darkAttack = default;

        public int physicsDefense = default;

        public int magicDefense = default;

        public int fireDefense = default;

        public int earthDefense = default;

        public int thunderDefense = default;

        public int waterDefense = default;

        public int holyDefense = default;

        public int darkDefense = default;

        public float physicsCutRate = default;

        public float magicCutRate = default;

        public float fireCutRate = default;

        public float earthCutRate = default;

        public float thunderCutRate = default;

        public float waterCutRate = default;

        public float holyCutRate = default;

        public float darkCutRate = default;

        public string LocalizedName => LocalizationManager.GetTermTranslation(this.name);
    }
}
