using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnemyLifecycleContoller : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;

        private void Start()
        {
            this.actor.Event.OnDeadSubject()
                .Subscribe(_ =>
                {
                    Destroy(this.actor.gameObject);
                })
                .AddTo(this);
        }
    }
}
