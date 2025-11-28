namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class DoesNotReturnIfAttribute(bool parameterValue) : Attribute
{
	public bool ParameterValue { get; } = parameterValue;
}
