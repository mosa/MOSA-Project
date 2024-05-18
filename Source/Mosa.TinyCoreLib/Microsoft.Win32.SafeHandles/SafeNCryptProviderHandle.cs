using System.Runtime.Versioning;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeNCryptProviderHandle : SafeNCryptHandle
{
	[SupportedOSPlatform("windows")]
	public SafeNCryptProviderHandle()
	{
	}

	protected override bool ReleaseNativeHandle()
	{
		throw null;
	}
}
