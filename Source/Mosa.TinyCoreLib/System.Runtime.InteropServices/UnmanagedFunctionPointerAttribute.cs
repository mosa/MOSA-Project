namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
public sealed class UnmanagedFunctionPointerAttribute : Attribute
{
	public bool BestFitMapping;

	public CharSet CharSet;

	public bool SetLastError;

	public bool ThrowOnUnmappableChar;

	public CallingConvention CallingConvention
	{
		get
		{
			throw null;
		}
	}

	public UnmanagedFunctionPointerAttribute(CallingConvention callingConvention)
	{
	}
}
