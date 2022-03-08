using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SubclassSelectorAttribute : PropertyAttribute
{
	Type m_Type;

	bool m_includeMono;

	public SubclassSelectorAttribute(Type type, bool includeMono = false)
	{
		m_Type = type;
		m_includeMono = includeMono;
	}

	public Type GetTargetType() => m_Type;

	public bool IsIncludeMono()
	{
		return m_includeMono;
	}
}