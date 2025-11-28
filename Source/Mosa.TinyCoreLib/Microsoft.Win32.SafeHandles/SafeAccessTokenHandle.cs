using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeAccessTokenHandle : SafeHandle
{
	public static SafeAccessTokenHandle InvalidHandle
	{
		get
		{
			throw null;
		}
	}

	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	public SafeAccessTokenHandle()
		: base((IntPtr)0, ownsHandle: false)
	{
	}

	public SafeAccessTokenHandle(IntPtr handle)
		: base((IntPtr)0, ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
