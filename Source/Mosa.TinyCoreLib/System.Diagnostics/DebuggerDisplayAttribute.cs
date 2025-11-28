namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Delegate, AllowMultiple = true)]
public sealed class DebuggerDisplayAttribute : Attribute
{
	public string? Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Type? Target
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? TargetTypeName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Value
	{
		get
		{
			throw null;
		}
	}

	public DebuggerDisplayAttribute(string? value)
	{
	}
}
