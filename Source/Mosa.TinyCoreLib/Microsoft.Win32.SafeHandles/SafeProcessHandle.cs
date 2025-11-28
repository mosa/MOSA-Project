using System;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public SafeProcessHandle()
		: base(ownsHandle: false)
	{
	}

	public SafeProcessHandle(IntPtr existingHandle, bool ownsHandle)
		: base(ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
