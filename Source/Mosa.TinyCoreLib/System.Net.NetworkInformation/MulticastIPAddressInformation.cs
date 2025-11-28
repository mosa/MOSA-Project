using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public abstract class MulticastIPAddressInformation : IPAddressInformation
{
	[SupportedOSPlatform("windows")]
	public abstract long AddressPreferredLifetime { get; }

	[SupportedOSPlatform("windows")]
	public abstract long AddressValidLifetime { get; }

	[SupportedOSPlatform("windows")]
	public abstract long DhcpLeaseLifetime { get; }

	[SupportedOSPlatform("windows")]
	public abstract DuplicateAddressDetectionState DuplicateAddressDetectionState { get; }

	[SupportedOSPlatform("windows")]
	public abstract PrefixOrigin PrefixOrigin { get; }

	[SupportedOSPlatform("windows")]
	public abstract SuffixOrigin SuffixOrigin { get; }
}
