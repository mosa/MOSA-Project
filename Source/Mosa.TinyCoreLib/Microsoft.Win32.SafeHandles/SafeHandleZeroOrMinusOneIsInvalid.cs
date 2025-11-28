using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public abstract class SafeHandleZeroOrMinusOneIsInvalid : SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	protected SafeHandleZeroOrMinusOneIsInvalid(bool ownsHandle)
		: base((IntPtr)0, ownsHandle: false)
	{
	}
}
