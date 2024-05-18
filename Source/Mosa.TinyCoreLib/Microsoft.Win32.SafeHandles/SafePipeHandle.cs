using System;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafePipeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	public SafePipeHandle()
		: base(ownsHandle: false)
	{
	}

	public SafePipeHandle(IntPtr preexistingHandle, bool ownsHandle)
		: base(ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
