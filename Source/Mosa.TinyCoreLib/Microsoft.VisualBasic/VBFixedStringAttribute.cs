using System;

namespace Microsoft.VisualBasic;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class VBFixedStringAttribute : Attribute
{
	public int Length
	{
		get
		{
			throw null;
		}
	}

	public VBFixedStringAttribute(int Length)
	{
	}
}
