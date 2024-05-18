namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
public sealed class ExcludeFromCodeCoverageAttribute : Attribute
{
	public string? Justification
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
