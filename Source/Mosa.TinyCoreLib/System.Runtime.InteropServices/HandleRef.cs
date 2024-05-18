namespace System.Runtime.InteropServices;

public readonly struct HandleRef
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public IntPtr Handle
	{
		get
		{
			throw null;
		}
	}

	public object? Wrapper
	{
		get
		{
			throw null;
		}
	}

	public HandleRef(object? wrapper, IntPtr handle)
	{
		throw null;
	}

	public static explicit operator IntPtr(HandleRef value)
	{
		throw null;
	}

	public static IntPtr ToIntPtr(HandleRef value)
	{
		throw null;
	}
}
