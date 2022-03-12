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
        /// <inheritdoc cref="onSpawnedActor"/>
        /// </summary>
        public static ISubject<Actor> OnSpawnedActorSubject() => onSpawnedActor;

        public static void Initialize()
        {
            onSpawnedActor = new Subject<Actor>();
        }

        public static void Clear()
        {
            onSpawnedActor.Dispose();
            onSpawnedActor = null;
        }
    }
}
