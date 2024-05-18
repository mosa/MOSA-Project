using System;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public SafeRegistryHandle()
		: base(ownsHandle: false)
	{
	}

	public SafeRegistryHandle(IntPtr preexistingHandle, bool ownsHandle)
		: base(ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
