using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public abstract class IPGlobalStatistics
{
	[UnsupportedOSPlatform("android")]
	public abstract int DefaultTtl { get; }

	[UnsupportedOSPlatform("android")]
	public abstract bool ForwardingEnabled { get; }

	public abstract int NumberOfInterfaces { get; }

	public abstract int NumberOfIPAddresses { get; }

	[UnsupportedOSPlatform("android")]
	public abstract int NumberOfRoutes { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long OutputPacketRequests { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long OutputPacketRoutingDiscards { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long OutputPacketsDiscarded { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long OutputPacketsWithNoRoute { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long PacketFragmentFailures { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long PacketReassembliesRequired { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long PacketReassemblyFailures { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long PacketReassemblyTimeout { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long PacketsFragmented { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long PacketsReassembled { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long ReceivedPackets { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long ReceivedPacketsDelivered { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long ReceivedPacketsDiscarded { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long ReceivedPacketsForwarded { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long ReceivedPacketsWithAddressErrors { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long ReceivedPacketsWithHeadersErrors { get; }

	[UnsupportedOSPlatform("android")]
	public abstract long ReceivedPacketsWithUnknownProtocol { get; }
}
