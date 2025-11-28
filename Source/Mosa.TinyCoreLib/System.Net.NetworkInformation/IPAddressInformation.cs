using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public abstract class IPAddressInformation
{
	public abstract IPAddress Address { get; }

	[SupportedOSPlatform("windows")]
	public abstract bool IsDnsEligible { get; }

	[SupportedOSPlatform("windows")]
	public abstract bool IsTransient { get; }
}
