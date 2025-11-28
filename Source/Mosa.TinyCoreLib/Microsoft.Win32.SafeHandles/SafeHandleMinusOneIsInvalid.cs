using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public abstract class SafeHandleMinusOneIsInvalid : SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	protected SafeHandleMinusOneIsInvalid(bool ownsHandle)
		: base((IntPtr)0, ownsHandle: false)
	{
	}
}
