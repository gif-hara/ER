using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using ER.MasterDataSystem;
using System.Collections.Generic;
using ER.StageControllers;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnemyLifecycleContoller : MonoBehaviour
    {
        [SerializeField]
        private Actor actor = default;

        [SerializeField]
        private PoolableEffect deadEffectPrefab = default;

        private void Start()
        {
            this.actor.Broker.Receive<ActorEvent.OnDead>()
                .Subscribe(async _ =>
                {
                    // エフェクト生成
                    var t = this.actor.transform;
                    this.deadEffectPrefab.Rent(t.position, t.rotation);
                    
                    // プレイヤーに経験値を付与
                    foreach (var i in Actor.Players)
                    {
                        i.StatusController.AddExperience(this.actor.StatusController.Experience);
                    }

                    // 確率でアイテムドロップ
                    var dropItems = new List<InteractableStageGimmickItem.Element>();
                    foreach (var i in MasterDataActorDropItem.Instance.Table[this.actor.Id])
                    {
                        if (i.Lottery())
                        {
                            dropItems.Add(new InteractableStageGimmickItem.Element
                            {
                                itemId = i.ItemId,
                                number = 1
                            });
                        }
                    }
                    if (dropItems.Count > 0)
                    {
                        GameController.Instance.Broker.Publish(GameEvent.OnRequestItemSpawn.Get(
                            this.actor.transform.position,
                            dropItems,
                            null
                        ));
                    }


                    Destroy(this.actor.gameObject);
                })
                .AddTo(this);
        }
    }
}
