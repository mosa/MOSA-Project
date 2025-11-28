using Internal;

namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class DebuggerBrowsableAttribute : Attribute
{
	public DebuggerBrowsableState State { get; }

	public DebuggerBrowsableAttribute(DebuggerBrowsableState state)
	{
		if (state is < DebuggerBrowsableState.Never or > DebuggerBrowsableState.RootHidden)
			Exceptions.Generic.ParameterOutOfRange(nameof(state));

		State = state;
	}
}
