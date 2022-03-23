using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorParentSetter : MonoBehaviour
    {
        [SerializeField]
        private Transform playerParent = default;

        [SerializeField]
        private Transform enemyParent = default;

        private void Awake()
        {
            GameController.Instance.Broker.Receive<GameEvent.OnSpawnedActor>()
                .Subscribe(x =>
                {
                    var layer = x.SpawnedActor.gameObject.layer;
                    if (layer == Layer.Index.Player)
                    {
                        x.SpawnedActor.transform.parent = this.playerParent;
                    }
                    else if (layer == Layer.Index.Enemy)
                    {
                        x.SpawnedActor.transform.parent = this.enemyParent;
                    }
                    else
                    {
                        Debug.LogWarning($"layer = {layer}は対応していません");
                    }
                })
                .AddTo(this);
        }
    }
}
