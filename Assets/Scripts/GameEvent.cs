using ER.ActorControllers;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class GameEvent
    {
        /// <summary>
        /// ゲームを開始出来る状態か
        /// </summary>
        public static ReactiveProperty<bool> IsGameReady { get; private set; }

        /// <summary>
        /// <see cref="Actor"/>が生成された際のイベント
        /// </summary>
        private static Subject<Actor> onSpawnedActor;

        /// <summary>
        /// <see cref="GameCameraController"/>が生成された際のイベント
        /// </summary>
        private static Subject<GameCameraController> onSpawnedGameCameraController;

        /// <summary>
        /// ゲームメニューの表示をリクエストするイベント
        /// </summary>
        private static Subject<Unit> onRequestOpenIngameMenu;

        /// <summary>
        /// <inheritdoc cref="onSpawnedActor"/>
        /// </summary>
        public static ISubject<Actor> OnSpawnedActorSubject() => onSpawnedActor;

        /// <summary>
        /// <inheritdoc cref="onSpawnedGameCameraController"/>
        /// </summary>
        public static ISubject<GameCameraController> OnSpawnedGameCameraControllerSubject() => onSpawnedGameCameraController;

        /// <summary>
        /// <inheritdoc cref="onRequestOpenIngameMenu"/>
        /// </summary>
        public static ISubject<Unit> OnRequestOpenIngameMenuSubject() => onRequestOpenIngameMenu;

        public static void Initialize()
        {
            IsGameReady = new ReactiveProperty<bool>(false);
            onSpawnedActor = new Subject<Actor>();
            onSpawnedGameCameraController = new Subject<GameCameraController>();
            onRequestOpenIngameMenu = new Subject<Unit>();
        }

        public static void Clear()
        {
            IsGameReady.Dispose();
            IsGameReady = null;
            onSpawnedActor.Dispose();
            onSpawnedActor = null;
            onSpawnedGameCameraController.Dispose();
            onSpawnedGameCameraController = null;
            onRequestOpenIngameMenu.Dispose();
            onRequestOpenIngameMenu = null;
        }
    }
}
