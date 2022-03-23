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
            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
                .Where(x => x.SpawnedActor.gameObject.layer == Layer.Index.Player)
                .Subscribe(x =>
                {
                    this.SetActiveVirtualCamera(this.defaultVirtualCamera);
                    this.SetDefaultVirtualCameraTarget(x.SpawnedActor.transform);
                    this.RegisterActorEvent(x.SpawnedActor);
                })
                .AddTo(this);
        }

        private void Start()
        {
            GameController.Instance.Broker.Publish(GameEvent.OnSpawnedGameCameraController.Get(this));
        }

        public void SetDefaultVirtualCameraTarget(Transform target)
        {
            this.defaultVirtualCamera.Follow = target;
            this.defaultVirtualCamera.LookAt = target;
        }

        private void RegisterActorEvent(IActor actor)
        {
            actor.Broker.Receive<ActorEvent.OnBeginLookAt>()
                .Subscribe(x =>
                {
                    SetActiveVirtualCamera(this.lookAtVirtualCamera);
                    this.lookAtTargetGroup.AddMember(actor.transform, 1.0f, 1.0f);
                    this.lookAtTargetGroup.AddMember(x.Target.transform, 1.0f, 1.0f);
                })
                .AddTo(this);

            actor.Broker.Receive<ActorEvent.OnEndLookAt>()
                .Subscribe(x =>
                {
                    SetActiveVirtualCamera(this.defaultVirtualCamera);
                    if (x.Target != null)
                    {
                        this.lookAtTargetGroup.RemoveMember(x.Target.transform);
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
