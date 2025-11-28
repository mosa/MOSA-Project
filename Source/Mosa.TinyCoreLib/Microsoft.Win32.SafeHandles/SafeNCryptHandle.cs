using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Microsoft.Win32.SafeHandles;

public abstract class SafeNCryptHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	[SupportedOSPlatform("windows")]
	protected SafeNCryptHandle()
		: base(ownsHandle: false)
	{
	}

	[SupportedOSPlatform("windows")]
	protected SafeNCryptHandle(IntPtr handle, SafeHandle parentHandle)
		: base(ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}

	protected abstract bool ReleaseNativeHandle();
}
