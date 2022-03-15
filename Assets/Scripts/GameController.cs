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
        [SerializeField]
        private GameCameraController gameCameraControllerPrefab = default;

        [SerializeField]
        private Actor playerPrefab = default;

        [SerializeField]
        private Transform playerSpawnPoint = default;

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
            GameEvent.Initialize();

            yield return MasterData.SetupAsync().ToYieldInstruction();

            Instantiate(gameCameraControllerPrefab);
            var player = Instantiate(this.playerPrefab, this.playerSpawnPoint.localPosition, this.playerSpawnPoint.localRotation);

            GameEvent.IsGameReady.Value = true;

            yield break;
        }
    }
}
