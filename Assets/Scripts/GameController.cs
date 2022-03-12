using ER.ActorControllers;
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
        private Actor playerPrefab = default;

        [SerializeField]
        private Transform playerSpawnPoint = default;

        private void Awake()
        {
            Instantiate(this.playerPrefab, this.playerSpawnPoint.localPosition, this.playerSpawnPoint.localRotation);
        }
    }
}
