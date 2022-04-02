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
                this.playerSpawnPoint.position,
                this.playerSpawnPoint.localRotation,
                this.playerActorStatusId
                );

            this.IsGameReady.Value = true;

            yield break;
        }
    }
}
