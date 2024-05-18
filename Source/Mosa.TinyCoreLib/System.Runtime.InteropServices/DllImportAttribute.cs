namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class DllImportAttribute : Attribute
{
	public bool BestFitMapping;

	public CallingConvention CallingConvention;

	public CharSet CharSet;

	public string? EntryPoint;

	public bool ExactSpelling;

	public bool PreserveSig;

	public bool SetLastError;

	public bool ThrowOnUnmappableChar;

	public string Value
	{
		get
		{
			throw null;
		}
	}

	public DllImportAttribute(string dllName)
	{
	}
}
