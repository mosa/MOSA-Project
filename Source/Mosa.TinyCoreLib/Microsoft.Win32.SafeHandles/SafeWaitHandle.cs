using System;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeWaitHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public SafeWaitHandle()
		: base(ownsHandle: false)
	{
	}

	public SafeWaitHandle(IntPtr existingHandle, bool ownsHandle)
		: base(ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
