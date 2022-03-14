using ER.ActorControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.StageControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorSpawnerOnStart : MonoBehaviour
    {
        [SerializeField]
        private Actor actorPrefab = default;

        private void Start()
        {
            Instantiate(this.actorPrefab, this.transform.position, this.transform.rotation);
        }
    }
}
