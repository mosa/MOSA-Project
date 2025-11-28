namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class MaybeNullWhenAttribute(bool returnValue) : Attribute
{
	public bool ReturnValue { get; } = returnValue;
}
