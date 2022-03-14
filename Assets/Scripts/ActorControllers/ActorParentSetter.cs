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
            GameEvent.OnSpawnedActorSubject()
                .Subscribe(x =>
                {
                    var layer = x.gameObject.layer;
                    if (layer == Layer.Index.Player)
                    {
                        x.transform.parent = this.playerParent;
                    }
                    else if (layer == Layer.Index.Enemy)
                    {
                        x.transform.parent = this.enemyParent;
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
