using UnityEngine;
using UnityEngine.SceneManagement;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public class LoadSceneAwake : MonoBehaviour
    {
        [SerializeField]
        private string sceneName = default;
        
        private void Awake()
        {
            SceneManager.LoadScene(this.sceneName);
        }
    }
}
