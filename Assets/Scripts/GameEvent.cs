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
        public class OnRequestOpenIngameMenu : Message<OnRequestOpenIngameMenu>
        {
        }
    }
}
