using UnityEngine;
using UnityEngine.SceneManagement;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public class LoadGameSceneAwake : MonoBehaviour
    {
        [SerializeField]
        private Transform playerSpawnPoint = default;
        
        private void Awake()
        {
            var sceneContext = new GameSceneContext
            {
                canOverrideStartTransform = true,
                startPosition = this.playerSpawnPoint.position,
                startRotation = this.playerSpawnPoint.localRotation
            };
            
            SceneContext.Set(sceneContext);
            
            SceneManager.LoadScene("Game");
        }
    }
}
