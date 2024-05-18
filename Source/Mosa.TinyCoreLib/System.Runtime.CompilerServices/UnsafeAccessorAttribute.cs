namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class UnsafeAccessorAttribute : Attribute
{
	public UnsafeAccessorKind Kind { get; }

	public string? Name { get; set; }

	public UnsafeAccessorAttribute(UnsafeAccessorKind kind)
	{
	}
}
