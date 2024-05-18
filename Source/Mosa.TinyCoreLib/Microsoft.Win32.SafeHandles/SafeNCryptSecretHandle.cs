using System.Runtime.Versioning;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeNCryptSecretHandle : SafeNCryptHandle
{
	[SupportedOSPlatform("windows")]
	public SafeNCryptSecretHandle()
	{
	}

	protected override bool ReleaseNativeHandle()
	{
		throw null;
	}
}
