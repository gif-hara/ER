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

        public ERInputAction InputAction { get; private set; }

        private void Awake()
        {
            StartCoroutine(this.SetupCoroutine());
        }

        private void OnDestroy()
        {
            GameEvent.Clear();
        }

        private IEnumerator SetupCoroutine()
        {
            Instance = this;
            this.InputAction = new ERInputAction();
            this.InputAction.Player.Enable();

            GameEvent.Initialize();

            yield return MasterData.SetupAsync().ToYieldInstruction();

            Instantiate(gameCameraControllerPrefab);
            var masterDataActorStatus = MasterDataActorStatus.Get(playerActorStatusId);
            var player = masterDataActorStatus.actorPrefab.Spawn(
                this.playerSpawnPoint.position,
                this.playerSpawnPoint.localRotation,
                masterDataActorStatus.statusData
                );

            GameEvent.IsGameReady.Value = true;

            yield break;
        }
    }
}
