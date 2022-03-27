using System;
using System.Collections.Generic;
using ER.ActorControllers;
using HK.Framework.EventSystems;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public class GameEvent
    {
        /// <summary>
        /// <see cref="Actor"/>が生成された際のイベント
        /// </summary>
        public class OnSpawnedActor : Message<OnSpawnedActor, Actor>
        {
            public Actor SpawnedActor => this.param1;
        }

        /// <summary>
        /// <see cref="GameCameraController"/>が生成された際のメッセージ
        /// </summary>
        public class OnSpawnedGameCameraController : Message<OnSpawnedGameCameraController, GameCameraController>
        {
            public GameCameraController SpawnedGameCameraController => this.param1;
        }

        /// <summary>
        /// ゲームメニューの表示をリクエストするメッセージ
        /// </summary>
        public class OnRequestOpenIngameMenu : Message<OnRequestOpenIngameMenu, IngameMenuType>
        {
            /// <summary>
            /// 開きたいメニュータイプ
            /// </summary>
            public IngameMenuType IngameMenuType => this.param1;
        }

        /// <summary>
        /// 装備品切り替えUIの表示をリクエストするメッセージ
        /// </summary>
        public class OnRequestOpenChangeEquipment : Message<OnRequestOpenChangeEquipment>
        {
        }

        /// <summary>
        /// インベントリUIの表示をリクエストするメッセージ
        /// </summary>
        public class OnRequestOpenInventory : Message<OnRequestOpenInventory, List<Item>, Action<Item>>
        {
            /// <summary>
            /// UIに表示したいアイテムのリスト
            /// </summary>
            public List<Item> TargetItems => this.param1;

            /// <summary>
            /// アイテムを選択した際の処理
            /// </summary>
            public Action<Item> OnSelectItemAction => this.param2;
        }
    }
}
