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
        /// <see cref="Actor"/>が生成された際のイベント
        /// </summary>
        private static Subject<Actor> onSpawnedActor;

        /// <summary>
        /// <see cref="GameCameraController"/>が生成された際のイベント
        /// </summary>
        private static Subject<GameCameraController> onSpawnedGameCameraController;

        /// <summary>
        /// <inheritdoc cref="onSpawnedActor"/>
        /// </summary>
        public static ISubject<Actor> OnSpawnedActorSubject() => onSpawnedActor;

        /// <summary>
        /// <inheritdoc cref="onSpawnedGameCameraController"/>
        /// </summary>
        public static ISubject<GameCameraController> OnSpawnedGameCameraController() => onSpawnedGameCameraController;

        public static void Initialize()
        {
            onSpawnedActor = new Subject<Actor>();
            onSpawnedGameCameraController = new Subject<GameCameraController>();
        }

        public static void Clear()
        {
            onSpawnedActor.Dispose();
            onSpawnedActor = null;
        }
    }
}
