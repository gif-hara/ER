using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

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

        private void Awake()
        {
            GameEvent.OnSpawnedActorSubject()
                .Where(x => x.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    SetDefaultVirtualCameraTarget(x.transform);
                })
                .AddTo(this);
        }

        public void SetDefaultVirtualCameraTarget(Transform target)
        {
            this.defaultVirtualCamera.Follow = target;
            this.defaultVirtualCamera.LookAt = target;
        }
    }
}
