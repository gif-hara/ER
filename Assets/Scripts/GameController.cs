using ER.ActorControllers;
using ER.MasterDataSystem;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField]
        private GameCameraController gameCameraControllerPrefab = default;

        [SerializeField]
        private string playerActorStatusId = default;

        [SerializeField]
        private Transform playerSpawnPoint = default;

        private MessageBroker broker = new MessageBroker();

        public ERInputAction InputAction { get; private set; }

        public IMessageBroker Broker => this.broker;

        /// <summary>
        /// ゲームを開始出来る状態か
        /// </summary>
        public ReactiveProperty<bool> IsGameReady { get; } = new ReactiveProperty<bool>(false);

        private void Awake()
        {
            StartCoroutine(this.SetupCoroutine());
        }

        private IEnumerator SetupCoroutine()
        {
            Instance = this;
            this.InputAction = new ERInputAction();
            this.InputAction.Player.Enable();

            yield return MasterData.SetupAsync().ToYieldInstruction();

            Instantiate(gameCameraControllerPrefab);
            var masterDataActorStatus = MasterDataActorStatus.Get(playerActorStatusId);
            var player = masterDataActorStatus.actorPrefab.Spawn(
                this.GetPlayerStartPosition(),
                this.GetPlayerStartRotation(),
                this.playerActorStatusId
                );

            this.Broker.Publish(GameEvent.OnRequestOpenInputTutorial.Get());
            
            this.IsGameReady.Value = true;
        }

        private Vector3 GetPlayerStartPosition()
        {
            var sceneContext = SceneContext.GetOrNull<GameSceneContext>();
            if (sceneContext != null && sceneContext.canOverrideStartTransform)
            {
                return sceneContext.startPosition;
            }

            return this.playerSpawnPoint.position;
        }

        private Quaternion GetPlayerStartRotation()
        {
            var sceneContext = SceneContext.GetOrNull<GameSceneContext>();
            if (sceneContext != null && sceneContext.canOverrideStartTransform)
            {
                return sceneContext.startRotation;
            }

            return this.playerSpawnPoint.localRotation;
        }
    }
}
