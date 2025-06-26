namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
public sealed class MethodImplAttribute : Attribute
{
	public MethodCodeType MethodCodeType;

	public MethodImplOptions Value { get; }

	public MethodImplAttribute() {}

	public MethodImplAttribute(short value) => Value = (MethodImplOptions)value;

	public MethodImplAttribute(MethodImplOptions methodImplOptions) => Value = methodImplOptions;
}
