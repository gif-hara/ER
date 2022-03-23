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

        public GameEvent Event { get; } = new GameEvent();

        public ERInputAction InputAction { get; private set; }

        public IMessageBroker Broker => this.broker;

        private void Awake()
        {
            StartCoroutine(this.SetupCoroutine());
        }

        private void OnDestroy()
        {
            this.Event.Dispose();
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
                this.playerSpawnPoint.position,
                this.playerSpawnPoint.localRotation,
                masterDataActorStatus.statusData
                );

            this.Event.IsGameReady.Value = true;

            yield break;
        }
    }
}
