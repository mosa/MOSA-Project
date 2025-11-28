using System;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeFileHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	public bool IsAsync
	{
		get
		{
			throw null;
		}
	}

	public SafeFileHandle()
		: base(ownsHandle: false)
	{
	}

	public SafeFileHandle(IntPtr preexistingHandle, bool ownsHandle)
		: base(ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
