using System.Collections.Generic;

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
[CLSCompliant(false)]
public sealed class TupleElementNamesAttribute : Attribute
{
	public IList<string?> TransformNames
	{
		get
		{
			throw null;
		}
	}

	public TupleElementNamesAttribute(string?[] transformNames)
	{
	}
}
