using System;
using ER.ActorControllers;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public class GameEvent : IDisposable
    {
        /// <summary>
        /// ゲームを開始出来る状態か
        /// </summary>
        public ReactiveProperty<bool> IsGameReady { get; } = new ReactiveProperty<bool>(false);

        /// <summary>
        /// <see cref="Actor"/>が生成された際のイベント
        /// </summary>
        private readonly Subject<Actor> onSpawnedActor = new Subject<Actor>();

        /// <summary>
        /// <see cref="GameCameraController"/>が生成された際のイベント
        /// </summary>
        private readonly Subject<GameCameraController> onSpawnedGameCameraController = new Subject<GameCameraController>();

        /// <summary>
        /// ゲームメニューの表示をリクエストするイベント
        /// </summary>
        private readonly Subject<Unit> onRequestOpenIngameMenu = new Subject<Unit>();

        /// <summary>
        /// <inheritdoc cref="onSpawnedActor"/>
        /// </summary>
        public ISubject<Actor> OnSpawnedActorSubject() => onSpawnedActor;

        /// <summary>
        /// <inheritdoc cref="onSpawnedGameCameraController"/>
        /// </summary>
        public ISubject<GameCameraController> OnSpawnedGameCameraControllerSubject() => onSpawnedGameCameraController;

        /// <summary>
        /// <inheritdoc cref="onRequestOpenIngameMenu"/>
        /// </summary>
        public ISubject<Unit> OnRequestOpenIngameMenuSubject() => onRequestOpenIngameMenu;

        public void Dispose()
        {
            IsGameReady.Dispose();
            onSpawnedActor.Dispose();
            onSpawnedGameCameraController.Dispose();
            onRequestOpenIngameMenu.Dispose();
        }
    }
}
