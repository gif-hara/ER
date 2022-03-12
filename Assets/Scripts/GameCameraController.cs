using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameCameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera controlledCamera = default;

        [SerializeField]
        private CinemachineVirtualCamera defaultVirtualCamera = default;

        public void SetDefaultVirtualCameraTarget(Transform target)
        {
            this.defaultVirtualCamera.Follow = target;
            this.defaultVirtualCamera.LookAt = target;
        }
    }
}
