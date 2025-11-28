namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class UnmanagedCallersOnlyAttribute : Attribute
{
	public Type[]? CallConvs;

	public string? EntryPoint;
}
