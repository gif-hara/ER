using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ActorUtility
    {
        /// <summary>
        /// 一番近い<see cref="Actor"/>を返す
        /// </summary>
        public static Actor GetNearActor(ActorType actorType, Vector3 position)
        {
            Actor result = null;
            var list = GetActorList(actorType);
            var minDistance = float.MaxValue;
            foreach (var i in list)
            {
                var diff = i.transform.position - position;
                var distance = diff.magnitude;

                if (distance <= minDistance)
                {
                    minDistance = distance;
                    result = i;
                }
            }

            return result;
        }

        public static List<Actor> GetActorList(ActorType actorType)
        {
            switch(actorType)
            {
                case ActorType.All:
                    return Actor.Actors;
                case ActorType.Player:
                    return Actor.Players;
                case ActorType.Enemy:
                    return Actor.Enemies;
                default:
                    Assert.IsTrue(false, $"{actorType}は未実装です");
                    return null;
            }
        }
    }
}
