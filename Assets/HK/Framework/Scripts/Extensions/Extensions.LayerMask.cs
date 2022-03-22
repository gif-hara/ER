using UnityEngine;

namespace HK.Framework.Extensions
{
	/// <summary>
	/// LayerMask拡張クラス.
	/// </summary>
	public static partial class Extensions
	{
		public static bool IsIncluded(this LayerMask self, GameObject gameObject)
		{
			return (self.value & (1 << gameObject.layer)) > 0;
		}
	}
}
