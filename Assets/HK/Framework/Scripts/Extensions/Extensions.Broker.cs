using HK.Framework.EventSystems;
using UniRx;
using UnityEngine;

namespace HK.Framework.Extensions
{
	/// <summary>
	/// Brokerの拡張クラス
	/// </summary>
	public static partial class Extensions
	{
		public static IMessageBroker GetBroker(this Component self)
		{
			return Broker.GetGameObjectBroker(self.gameObject);
		}
	}
}
