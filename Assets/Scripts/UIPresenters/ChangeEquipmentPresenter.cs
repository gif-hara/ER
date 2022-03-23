using ER.ActorControllers;
using ER.MasterDataSystem;
using ER.UIViews;
using I2.Loc;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

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
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            buttons.SetupVerticalNavigations();

            for (var i = 0; i < Define.EquipmentableNumber; i++)
            {
                // 右手
                {
                    var buttonElement = this.changeEquipmentUIView.GetRightEquipmentButtonElement(i);
                    var equipmentController = this.actor.EquipmentController.RightHand.GetEquipmentController(i);

                    if (equipmentController == null)
                    {
                        buttonElement.Label.text = ScriptLocalization.Common.Empty;
                    }
                    else
                    {
                        var item = equipmentController.Item;
                        var masterDataItem = item.MasterDataItem;
                        buttonElement.Label.text = masterDataItem.LocalizedName;
                        buttonElement.Button.OnClickAsObservable()
                            .Subscribe(_ => Debug.Log("TODO"))
                            .AddTo(this.disposables);
                    }
                }
                // 左手
                {
                    var buttonElement = this.changeEquipmentUIView.GetLeftEquipmentButtonElement(i);
                    var equipmentController = this.actor.EquipmentController.LeftHand.GetEquipmentController(i);

                    if (equipmentController == null)
                    {
                        buttonElement.Label.text = ScriptLocalization.Common.Empty;
                    }
                    else
                    {
                        var item = equipmentController.Item;
                        var masterDataItem = item.MasterDataItem;
                        buttonElement.Label.text = masterDataItem.LocalizedName;
                        buttonElement.Button.OnClickAsObservable()
                            .Subscribe(_ => Debug.Log("TODO"))
                            .AddTo(this.disposables);
                    }
                }
            }

            // 防具
            {
                this.ApplyArmor(ArmorType.Head);
                this.ApplyArmor(ArmorType.Torso);
                this.ApplyArmor(ArmorType.Arm);
                this.ApplyArmor(ArmorType.Leg);
            }
        }

        private void ApplyArmor(ArmorType armorType)
        {
            var buttonElement = this.changeEquipmentUIView.GetArmorButtonElement(armorType);
            var armorItem = this.actor.EquipmentController.GetArmorItem(armorType);

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
    }
}
