namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class UnmanagedCallConvAttribute : Attribute
{
	public Type[]? CallConvs;
}
