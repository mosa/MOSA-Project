using System.Collections.Generic;

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public sealed class DynamicAttribute : Attribute
{
	public IList<bool> TransformFlags
	{
		get
		{
			throw null;
		}
	}

	public DynamicAttribute()
	{
	}

	public DynamicAttribute(bool[] transformFlags)
	{
	}
}
