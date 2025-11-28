using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public abstract class IPv4InterfaceProperties
{
	public abstract int Index { get; }

	[SupportedOSPlatform("windows")]
	public abstract bool IsAutomaticPrivateAddressingActive { get; }

	[SupportedOSPlatform("windows")]
	public abstract bool IsAutomaticPrivateAddressingEnabled { get; }

	[SupportedOSPlatform("windows")]
	public abstract bool IsDhcpEnabled { get; }

	[SupportedOSPlatform("windows")]
	[SupportedOSPlatform("linux")]
	public abstract bool IsForwardingEnabled { get; }

	public abstract int Mtu { get; }

	[SupportedOSPlatform("windows")]
	[SupportedOSPlatform("linux")]
	public abstract bool UsesWins { get; }
}
