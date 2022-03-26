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
            var items = new List<Item>();

            var buttons = this.changeEquipmentUIView.GetAllButtons();
            this.buttonGameObjects = buttons.Select(x => x.gameObject).ToList();
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            buttons.SetupVerticalNavigations();

            // 右手
            for (var i = 0; i < Define.EquipmentableNumber; i++)
            {
                var buttonElement = this.changeEquipmentUIView.GetRightEquipmentButtonElement(i);
                var equipmentController = this.actor.EquipmentController.RightHand.GetEquipmentController(i);
                var index = i;
                var item = equipmentController != null ? equipmentController.Item : null;

                items.Add(item);
                buttonElement.Label.text = item != null ? item.MasterDataItem.LocalizedName : ScriptLocalization.Common.Empty;
                buttonElement.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        var targetItems = this.actor.InventoryController.Equipments
                        .Where(x => x.Value.MasterDataItem.Category == ItemCategory.Weapon)
                        .Select(x => x.Value)
                        .ToList();
                        GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenInventory.Get(
                            targetItems,
                            selectedItem =>
                            {
                                // 同じアイテムを選択した場合は外す処理
                                if (item != null && item.InstanceId == selectedItem.InstanceId)
                                {
                                    // ただし全て外すことはできない
                                    if (this.actor.EquipmentController.RightHand.GetEquipmentNumber() == 1)
                                    {
                                        Debug.Log("TODO:装備品を全て外すことは不可能な旨を伝える");
                                    }
                                    else
                                    {
                                        this.actor.EquipmentController.RightHand.Remove(index);
                                        GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenChangeEquipment.Get());
                                    }
                                }
                                else
                                {
                                    // 新規の場合はアタッチする
                                    var selectedIndex = this.actor.EquipmentController.RightHand.Find(selectedItem.InstanceId);
                                    if (selectedIndex == -1)
                                    {
                                        this.actor.EquipmentController.RightHand.Attach(index, selectedItem.MasterDataItem.ToWeapon().EquipmentControllerPrefab, selectedItem.InstanceId);
                                    }
                                    // 装備済みの場合はインデックスをスワップする
                                    else
                                    {
                                        this.actor.EquipmentController.RightHand.Swap(index, selectedIndex);
                                    }
                                    GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenChangeEquipment.Get());
                                }
                            }));
                    })
                    .AddTo(this.disposables);
            }

            // 左手
            for (var i = 0; i < Define.EquipmentableNumber; i++)
            {
                var buttonElement = this.changeEquipmentUIView.GetLeftEquipmentButtonElement(i);
                var equipmentController = this.actor.EquipmentController.LeftHand.GetEquipmentController(i);
                var index = i;
                var item = equipmentController != null ? equipmentController.Item : null;
                items.Add(item);
                buttonElement.Label.text = item != null ? item.MasterDataItem.LocalizedName : ScriptLocalization.Common.Empty;
                buttonElement.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        var targetItems = this.actor.InventoryController.Equipments
                        .Where(x => x.Value.MasterDataItem.Category == ItemCategory.Shield)
                        .Select(x => x.Value)
                        .ToList();
                        GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenInventory.Get(
                            targetItems,
                            selectedItem =>
                            {
                                // 同じアイテムを選択した場合は外す処理
                                if (item != null && item.InstanceId == selectedItem.InstanceId)
                                {
                                    // ただし全て外すことはできない
                                    if (this.actor.EquipmentController.LeftHand.GetEquipmentNumber() == 1)
                                    {
                                        Debug.Log("TODO:装備品を全て外すことは不可能な旨を伝える");
                                    }
                                    else
                                    {
                                        this.actor.EquipmentController.LeftHand.Remove(index);
                                        GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenChangeEquipment.Get());
                                    }
                                }
                                else
                                {
                                    // 新規の場合はアタッチする
                                    var selectedIndex = this.actor.EquipmentController.LeftHand.Find(selectedItem.InstanceId);
                                    if (selectedIndex == -1)
                                    {
                                        this.actor.EquipmentController.LeftHand.Attach(index, selectedItem.MasterDataItem.ToShield().EquipmentControllerPrefab, selectedItem.InstanceId);
                                    }
                                    // 装備済みの場合はインデックスをスワップする
                                    else
                                    {
                                        this.actor.EquipmentController.LeftHand.Swap(index, selectedIndex);
                                    }
                                    GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenChangeEquipment.Get());
                                }
                            }));
                    })
                    .AddTo(this.disposables);
            }

            // 防具
            {
                this.ApplyArmor(ArmorType.Head, items);
                this.ApplyArmor(ArmorType.Torso, items);
                this.ApplyArmor(ArmorType.Arm, items);
                this.ApplyArmor(ArmorType.Leg, items);
            }

            EventSystem.current.ObserveEveryValueChanged(x => x.currentSelectedGameObject)
                .Select(x => this.buttonGameObjects.IndexOf(x))
                .Subscribe(x => this.index.Value = x)
                .AddTo(this.disposables);

            this.index
                .Subscribe(x =>
                {
                    UIUtility.ApplyInformation(this.changeEquipmentUIView.Information, this.actor, items[x]);
                })
                .AddTo(this.disposables);
        }

        public void Deactivate()
        {
            this.disposables.Clear();
        }

        private void ApplyArmor(ArmorType armorType, List<Item> items)
        {
            var buttonElement = this.changeEquipmentUIView.GetArmorButtonElement(armorType);
            var armorItem = this.actor.EquipmentController.GetArmorItem(armorType);
            items.Add(armorItem);
            buttonElement.Label.text = armorItem != null ? armorItem.MasterDataItem.LocalizedName : ScriptLocalization.Common.Empty;
            buttonElement.Button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    var targetItems = this.actor.InventoryController.Equipments
                    .Where(x => x.Value.MasterDataItem.Category == armorType.ToItemCategory())
                    .Select(x => x.Value)
                    .ToList();
                    GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenInventory.Get(
                        targetItems,
                        selectedItem =>
                        {
                            // 同じアイテムが選択された場合は外す
                            if (armorItem != null && armorItem.InstanceId == selectedItem.InstanceId)
                            {
                                this.actor.EquipmentController.RemoveArmorItem(armorType);
                            }
                            else
                            {
                                this.actor.EquipmentController.SetArmorItem(armorType, selectedItem.InstanceId);
                            }

                            GameController.Instance.Broker.Publish(GameEvent.OnRequestOpenChangeEquipment.Get());
                        }));
                })
                .AddTo(this);
        }
    }
}
