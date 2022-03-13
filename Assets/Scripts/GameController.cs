using ER.ActorControllers;
using ER.StageControllers;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

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
            GameEvent.Initialize();
            Instantiate(gameCameraControllerPrefab);
            var player = Instantiate(this.playerPrefab, this.playerSpawnPoint.localPosition, this.playerSpawnPoint.localRotation);

            var stageLoader = new StageLoader();
            stageLoader.LoadAsync(player.transform.position)
            .Subscribe();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                });
        }

        private void OnDestroy()
        {
            GameEvent.Clear();
        }
    }
}
