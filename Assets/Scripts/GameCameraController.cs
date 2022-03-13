using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using ER.ActorControllers;

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

        [SerializeField]
        private CinemachineVirtualCamera lookAtVirtualCamera = default;

        /// <summary>
        /// ロックオン時のFollowデータを持つクラス
        /// </summary>
        [SerializeField]
        private CinemachineTargetGroup lookAtTargetGroup = default;

        public Camera ControlledCamera => this.controlledCamera;

        private void Awake()
        {
            GameEvent.OnSpawnedActorSubject()
                .Where(x => x.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.SetActiveVirtualCamera(this.defaultVirtualCamera);
                    this.SetDefaultVirtualCameraTarget(x.transform);
                    this.RegisterActorEvent(x);
                })
                .AddTo(this);
        }

        private void Start()
        {
            GameEvent.OnSpawnedGameCameraController().OnNext(this);
        }

        public void SetDefaultVirtualCameraTarget(Transform target)
        {
            this.defaultVirtualCamera.Follow = target;
            this.defaultVirtualCamera.LookAt = target;
        }

        private void RegisterActorEvent(IActor actor)
        {
            actor.Event.OnBeginLookAtSubject()
                .Subscribe(x =>
                {
                    SetActiveVirtualCamera(this.lookAtVirtualCamera);
                    this.lookAtTargetGroup.AddMember(actor.transform, 1.0f, 1.0f);
                    this.lookAtTargetGroup.AddMember(x.transform, 1.0f, 1.0f);
                })
                .AddTo(this);

            actor.Event.OnEndLookAtSubject()
                .Subscribe(x =>
                {
                    SetActiveVirtualCamera(this.defaultVirtualCamera);
                    if(x != null)
                    {
                        this.lookAtTargetGroup.RemoveMember(x.transform);
                    }
                })
                .AddTo(this);
        }

        private void SetActiveVirtualCamera(CinemachineVirtualCamera virtualCamera)
        {
            this.defaultVirtualCamera.enabled = this.defaultVirtualCamera == virtualCamera;
            this.lookAtVirtualCamera.enabled = this.lookAtVirtualCamera == virtualCamera;
        }
    }
}
