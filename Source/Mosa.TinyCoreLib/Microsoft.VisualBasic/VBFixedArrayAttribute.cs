using System;

namespace Microsoft.VisualBasic;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class VBFixedArrayAttribute : Attribute
{
	public int[] Bounds
	{
		get
		{
			throw null;
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public VBFixedArrayAttribute(int UpperBound1)
	{
	}

	public VBFixedArrayAttribute(int UpperBound1, int UpperBound2)
	{
	}
}
