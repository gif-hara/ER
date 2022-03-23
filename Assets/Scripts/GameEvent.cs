using System;
using ER.ActorControllers;
using HK.Framework.EventSystems;
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
        /// <see cref="Actor"/>が生成された際のイベント
        /// </summary>
        public class OnSpawnedActor : Message<OnSpawnedActor, Actor>
        {
            public Actor SpawnedActor => this.param1;
        }

        /// <summary>
        /// <see cref="GameCameraController"/>が生成された際のイベント
        /// </summary>
        private readonly Subject<GameCameraController> onSpawnedGameCameraController = new Subject<GameCameraController>();

        /// <summary>
        /// ゲームメニューの表示をリクエストするイベント
        /// </summary>
        private readonly Subject<Unit> onRequestOpenIngameMenu = new Subject<Unit>();

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
            onSpawnedGameCameraController.Dispose();
            onRequestOpenIngameMenu.Dispose();
        }
    }
}
