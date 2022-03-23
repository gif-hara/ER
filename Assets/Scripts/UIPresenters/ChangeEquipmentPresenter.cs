using System.Collections.Generic;
using System.Linq;
using ER.ActorControllers;
using ER.MasterDataSystem;
using ER.UIViews;
using I2.Loc;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ER.UIPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChangeEquipmentPresenter : UIPresenter
    {
        [SerializeField]
        private ChangeEquipmentUIView changeEquipmentUIView = default;

        private Actor actor;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        private List<GameObject> buttonGameObjects;

        /// <summary>
        /// 現在選択中のインデックス
        /// </summary>
        private ReactiveProperty<int> index = new ReactiveProperty<int>(0);

        /// <summary>
        /// ビューに反映されているアイテムのリスト
        /// </summary>
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
                .Where(x => x.SpawnedActor.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.actor = x.SpawnedActor;
                })
                .AddTo(this);
        }

        public void Activate()
        {
            this.disposables.Clear();

            var buttons = this.changeEquipmentUIView.GetAllButtons();
            this.buttonGameObjects = buttons.Select(x => x.gameObject).ToList();
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            buttons.SetupVerticalNavigations();

            // 右手
            for (var i = 0; i < Define.EquipmentableNumber; i++)
            {
                var buttonElement = this.changeEquipmentUIView.GetRightEquipmentButtonElement(i);
                var equipmentController = this.actor.EquipmentController.RightHand.GetEquipmentController(i);

                if (equipmentController == null)
                {
                    this.items.Add(null);
                    buttonElement.Label.text = ScriptLocalization.Common.Empty;
                }
                else
                {
                    var item = equipmentController.Item;
                    this.items.Add(item);
                    var masterDataItem = item.MasterDataItem;
                    buttonElement.Label.text = masterDataItem.LocalizedName;
                    buttonElement.Button.OnClickAsObservable()
                        .Subscribe(_ => Debug.Log("TODO"))
                        .AddTo(this.disposables);
                }
            }

            // 左手
            for (var i = 0; i < Define.EquipmentableNumber; i++)
            {
                var buttonElement = this.changeEquipmentUIView.GetLeftEquipmentButtonElement(i);
                var equipmentController = this.actor.EquipmentController.LeftHand.GetEquipmentController(i);

                if (equipmentController == null)
                {
                    this.items.Add(null);
                    buttonElement.Label.text = ScriptLocalization.Common.Empty;
                }
                else
                {
                    var item = equipmentController.Item;
                    this.items.Add(item);
                    var masterDataItem = item.MasterDataItem;
                    buttonElement.Label.text = masterDataItem.LocalizedName;
                    buttonElement.Button.OnClickAsObservable()
                        .Subscribe(_ => Debug.Log("TODO"))
                        .AddTo(this.disposables);
                }
            }

            // 防具
            {
                this.ApplyArmor(ArmorType.Head);
                this.ApplyArmor(ArmorType.Torso);
                this.ApplyArmor(ArmorType.Arm);
                this.ApplyArmor(ArmorType.Leg);
            }

            EventSystem.current.ObserveEveryValueChanged(x => x.currentSelectedGameObject)
                .Select(x => this.buttonGameObjects.IndexOf(x))
                .Subscribe(x => this.index.Value = x)
                .AddTo(this.disposables);

            this.index
                .Subscribe(x =>
                {
                    this.ApplyInformation(this.items[x]);
                })
                .AddTo(this.disposables);
        }

        private void ApplyArmor(ArmorType armorType)
        {
            var buttonElement = this.changeEquipmentUIView.GetArmorButtonElement(armorType);
            var armorItem = this.actor.EquipmentController.GetArmorItem(armorType);
            this.items.Add(armorItem);

            if (armorItem == null)
            {
                buttonElement.Label.text = ScriptLocalization.Common.Empty;
            }
            else
            {
                buttonElement.Label.text = armorItem.MasterDataItem.LocalizedName;
                buttonElement.Button.OnClickAsObservable()
                    .Subscribe(_ => Debug.Log(armorItem.MasterDataItem.LocalizedName))
                    .AddTo(this);
            }
        }

        private void ApplyInformation(Item item)
        {
            if (item == null)
            {
                this.changeEquipmentUIView.Information.text = "";
                return;
            }

            switch (item.MasterDataItem.Category)
            {
                case ItemCategory.Weapon:
                    var masterDataWeapon = item.MasterDataItem.ToWeapon();
                    var levelData = this.actor.InventoryController.WeaponLevelDatabase[item.InstanceId];
                    this.changeEquipmentUIView.Information.text = string.Format(
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
                    this.changeEquipmentUIView.Information.text = string.Format(
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
                    this.changeEquipmentUIView.Information.text = string.Format(
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
                    this.changeEquipmentUIView.Information.text = "TODO";
                    break;
                default:
                    Assert.IsTrue(false, $"{item.MasterDataItem.Category}は未対応です");
                    break;
            }
        }
    }
}
