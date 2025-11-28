namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class NotNullWhenAttribute(bool returnValue) : Attribute
{
	public bool ReturnValue { get; } = returnValue;
}
