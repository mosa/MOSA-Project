namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
public sealed class NotNullIfNotNullAttribute(string parameterName) : Attribute
{
	public string ParameterName { get; } = parameterName;
}
