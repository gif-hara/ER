using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace HK.Framework.EventSystems
{
    /// <summary>
    /// メッセージを仲介するクラス
    /// </summary>
    public static class Broker
    {
        /// <summary>
        /// アプリケーション全体へ通知する<see cref="IMessageBroker"/>
        /// </summary>
        public static readonly IMessageBroker Global = MessageBroker.Default;
        
        /// <summary>
        /// <see cref="GameObject"/>に紐づく<see cref="IMessageBroker"/>
        /// </summary>
        private static readonly Dictionary<GameObject, IMessageBroker> gameObjectBrokers = new Dictionary<GameObject, IMessageBroker>();

        /// <summary>
        /// <paramref name="gameObject"/>に紐づく<see cref="IMessageBroker"/>を返す
        /// </summary>
        public static IMessageBroker GetGameObjectBroker(GameObject gameObject)
        {
            IMessageBroker broker;

            if (!gameObjectBrokers.TryGetValue(gameObject, out broker))
            {
                broker = new MessageBroker();
                gameObjectBrokers.Add(gameObject, broker);
            }

            return broker;
        }
    }
}
