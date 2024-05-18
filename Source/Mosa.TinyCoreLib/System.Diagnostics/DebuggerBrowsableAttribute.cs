namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class DebuggerBrowsableAttribute : Attribute
{
	public DebuggerBrowsableState State
	{
		get
		{
			throw null;
		}
	}

	public DebuggerBrowsableAttribute(DebuggerBrowsableState state)
	{
	}
}
