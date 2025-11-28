using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public abstract class CriticalHandleMinusOneIsInvalid : CriticalHandle
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	protected CriticalHandleMinusOneIsInvalid()
		: base((IntPtr)0)
	{
	}
}
