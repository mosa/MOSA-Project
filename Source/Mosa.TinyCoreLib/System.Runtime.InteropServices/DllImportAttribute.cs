namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class DllImportAttribute(string dllName) : Attribute
{
	public bool BestFitMapping;

	public CallingConvention CallingConvention;

	public CharSet CharSet;

	public string? EntryPoint;

	public bool ExactSpelling;

	public bool PreserveSig;

	public bool SetLastError;

	public bool ThrowOnUnmappableChar;

	public string Value { get; } = dllName;
}
