using Microsoft.Win32.SafeHandles;

namespace System.Net.Sockets;

public sealed class SafeSocketHandle : SafeHandleMinusOneIsInvalid
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	public SafeSocketHandle()
		: base(ownsHandle: false)
	{
	}

	public SafeSocketHandle(IntPtr preexistingHandle, bool ownsHandle)
		: base(ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
