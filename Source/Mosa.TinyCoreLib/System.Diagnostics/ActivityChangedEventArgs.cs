using System.Runtime.InteropServices;

namespace System.Diagnostics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct ActivityChangedEventArgs
{
	public Activity? Previous
	{
		get
		{
			throw null;
		}
		init
		{
			throw null;
		}
	}

	public Activity? Current
	{
		get
		{
			throw null;
		}
		init
		{
			throw null;
		}
	}
}
