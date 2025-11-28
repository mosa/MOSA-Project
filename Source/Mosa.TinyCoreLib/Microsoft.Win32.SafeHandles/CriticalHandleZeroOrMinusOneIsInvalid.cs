using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public abstract class CriticalHandleZeroOrMinusOneIsInvalid : CriticalHandle
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	protected CriticalHandleZeroOrMinusOneIsInvalid()
		: base((IntPtr)0)
	{
	}
}
