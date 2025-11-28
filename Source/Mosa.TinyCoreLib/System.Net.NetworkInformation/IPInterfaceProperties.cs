using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public abstract class IPInterfaceProperties
{
	[SupportedOSPlatform("windows")]
	public abstract IPAddressInformationCollection AnycastAddresses { get; }

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("freebsd")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("osx")]
	[UnsupportedOSPlatform("tvos")]
	public abstract IPAddressCollection DhcpServerAddresses { get; }

	[UnsupportedOSPlatform("android")]
	public abstract IPAddressCollection DnsAddresses { get; }

	[UnsupportedOSPlatform("android")]
	public abstract string DnsSuffix { get; }

	[UnsupportedOSPlatform("android")]
	public abstract GatewayIPAddressInformationCollection GatewayAddresses { get; }

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("freebsd")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("osx")]
	[UnsupportedOSPlatform("tvos")]
	public abstract bool IsDnsEnabled { get; }

	[SupportedOSPlatform("windows")]
	public abstract bool IsDynamicDnsEnabled { get; }

	public abstract MulticastIPAddressInformationCollection MulticastAddresses { get; }

	public abstract UnicastIPAddressInformationCollection UnicastAddresses { get; }

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("freebsd")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("osx")]
	[UnsupportedOSPlatform("tvos")]
	public abstract IPAddressCollection WinsServersAddresses { get; }

	public abstract IPv4InterfaceProperties GetIPv4Properties();

	public abstract IPv6InterfaceProperties GetIPv6Properties();
}
