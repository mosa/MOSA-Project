namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
public sealed class ExperimentalAttribute : Attribute
{
	public string DiagnosticId
	{
		get
		{
			throw null;
		}
	}

	public string? UrlFormat { get; set; }

	public ExperimentalAttribute(string diagnosticId)
	{
	}
}
