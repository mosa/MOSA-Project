namespace System;

[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
public sealed class CLSCompliantAttribute : Attribute
{
	public bool IsCompliant
	{
		get
		{
			throw null;
		}
	}

	public CLSCompliantAttribute(bool isCompliant)
	{
	}
}
