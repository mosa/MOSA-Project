namespace System;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public sealed class AttributeUsageAttribute : Attribute
{
	public bool AllowMultiple
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Inherited
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AttributeTargets ValidOn
	{
		get
		{
			throw null;
		}
	}

	public AttributeUsageAttribute(AttributeTargets validOn)
	{
	}
}
