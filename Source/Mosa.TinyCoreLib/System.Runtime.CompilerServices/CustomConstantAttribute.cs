namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
public abstract class CustomConstantAttribute : Attribute
{
	public abstract object? Value { get; }
}
