using UnityEngine;

namespace HK.Framework.Editor
{
	public abstract class EditorScriptableObject<T> : EditorBase where T : ScriptableObject
	{
		private T _target = null;
		
		protected T Target
		{
			get
			{
				if( _target == null )
				{
					_target = (T)target;
				}
				return _target;
			}
		}
	}
}
