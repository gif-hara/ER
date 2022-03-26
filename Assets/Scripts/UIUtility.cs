using ER.ActorControllers;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class UIUtility
    {
        public static void ApplyInformation(TextMeshProUGUI textMesh, Actor actor, Item item)
        {
            if (item == null)
            {
                textMesh.text = "";
                return;
            }

            switch (item.MasterDataItem.Category)
            {
                case ItemCategory.Weapon:
                    var masterDataWeapon = item.MasterDataItem.ToWeapon();
                    var levelData = actor.InventoryController.WeaponLevelDatabase[item.InstanceId];
                    textMesh.text = string.Format(
                        ScriptLocalization.Common.WeaponInformation,
                        masterDataWeapon.LocalizedName,
                        masterDataWeapon.GetAttackElement(AttackAttributeType.Physics).Evaluate(levelData.GetRate(AttackAttributeType.Physics)),
                        masterDataWeapon.GetAttackElement(AttackAttributeType.Magic).Evaluate(levelData.GetRate(AttackAttributeType.Magic)),
                        masterDataWeapon.GetAttackElement(AttackAttributeType.Fire).Evaluate(levelData.GetRate(AttackAttributeType.Fire)),
                        masterDataWeapon.GetAttackElement(AttackAttributeType.Earth).Evaluate(levelData.GetRate(AttackAttributeType.Earth)),
                        masterDataWeapon.GetAttackElement(AttackAttributeType.Thunder).Evaluate(levelData.GetRate(AttackAttributeType.Thunder)),
                        masterDataWeapon.GetAttackElement(AttackAttributeType.Water).Evaluate(levelData.GetRate(AttackAttributeType.Water)),
                        masterDataWeapon.GetAttackElement(AttackAttributeType.Holy).Evaluate(levelData.GetRate(AttackAttributeType.Holy)),
                        masterDataWeapon.GetAttackElement(AttackAttributeType.Dark).Evaluate(levelData.GetRate(AttackAttributeType.Dark))
                        );
                    break;
                case ItemCategory.Shield:
                    var masterDataShield = item.MasterDataItem.ToShield();
                    textMesh.text = string.Format(
                        ScriptLocalization.Common.ShieldInformation,
                        masterDataShield.LocalizedName,
                        masterDataShield.GetCutRate(AttackAttributeType.Physics).ToPercentage(),
                        masterDataShield.GetCutRate(AttackAttributeType.Magic).ToPercentage(),
                        masterDataShield.GetCutRate(AttackAttributeType.Fire).ToPercentage(),
                        masterDataShield.GetCutRate(AttackAttributeType.Earth).ToPercentage(),
                        masterDataShield.GetCutRate(AttackAttributeType.Thunder).ToPercentage(),
                        masterDataShield.GetCutRate(AttackAttributeType.Water).ToPercentage(),
                        masterDataShield.GetCutRate(AttackAttributeType.Holy).ToPercentage(),
                        masterDataShield.GetCutRate(AttackAttributeType.Dark).ToPercentage()
                        );
                    break;
                case ItemCategory.ArmorHead:
                case ItemCategory.ArmorTorso:
                case ItemCategory.ArmorArm:
                case ItemCategory.ArmorLeg:
                    var masterDataArmor = item.MasterDataItem.ToArmor();
                    textMesh.text = string.Format(
                        ScriptLocalization.Common.ArmorInformaiton,
                        masterDataArmor.LocalizedName,
                        masterDataArmor.GetDefense(AttackAttributeType.Physics),
                        masterDataArmor.GetDefense(AttackAttributeType.Magic),
                        masterDataArmor.GetDefense(AttackAttributeType.Fire),
                        masterDataArmor.GetDefense(AttackAttributeType.Earth),
                        masterDataArmor.GetDefense(AttackAttributeType.Thunder),
                        masterDataArmor.GetDefense(AttackAttributeType.Water),
                        masterDataArmor.GetDefense(AttackAttributeType.Holy),
                        masterDataArmor.GetDefense(AttackAttributeType.Dark)
                        );
                    break;
                case ItemCategory.Accessory:
                    textMesh.text = "TODO";
                    break;
                default:
                    Assert.IsTrue(false, $"{item.MasterDataItem.Category}は未対応です");
                    break;
            }
        }
    }
}
