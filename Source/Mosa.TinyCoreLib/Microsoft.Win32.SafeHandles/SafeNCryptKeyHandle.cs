using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeNCryptKeyHandle : SafeNCryptHandle
{
	[SupportedOSPlatform("windows")]
	public SafeNCryptKeyHandle()
	{
	}

	[SupportedOSPlatform("windows")]
	public SafeNCryptKeyHandle(IntPtr handle, SafeHandle parentHandle)
	{
	}

	protected override bool ReleaseNativeHandle()
	{
		throw null;
	}
}
