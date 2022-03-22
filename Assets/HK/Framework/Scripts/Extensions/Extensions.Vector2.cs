using UnityEngine;

namespace HK.Framework.Extensions
{
	/// <summary>
	/// <see cref="Vector2">に関する拡張関数群
	/// </summary>
	public static partial class Extensions
	{
		public static float Angle(this Vector2 self)
		{
            return Vector2.Angle(Vector2.zero, self.normalized);
        }
	}
}