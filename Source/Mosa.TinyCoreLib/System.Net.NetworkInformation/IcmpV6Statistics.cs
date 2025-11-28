using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public abstract class IcmpV6Statistics
{
	public abstract long DestinationUnreachableMessagesReceived { get; }

	public abstract long DestinationUnreachableMessagesSent { get; }

	public abstract long EchoRepliesReceived { get; }

	public abstract long EchoRepliesSent { get; }

	public abstract long EchoRequestsReceived { get; }

	public abstract long EchoRequestsSent { get; }

	[UnsupportedOSPlatform("freebsd")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("osx")]
	[UnsupportedOSPlatform("tvos")]
	public abstract long ErrorsReceived { get; }

	[UnsupportedOSPlatform("freebsd")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("osx")]
	[UnsupportedOSPlatform("tvos")]
	public abstract long ErrorsSent { get; }

	public abstract long MembershipQueriesReceived { get; }

	public abstract long MembershipQueriesSent { get; }

	public abstract long MembershipReductionsReceived { get; }

	public abstract long MembershipReductionsSent { get; }

	public abstract long MembershipReportsReceived { get; }

	public abstract long MembershipReportsSent { get; }

	[UnsupportedOSPlatform("freebsd")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("osx")]
	[UnsupportedOSPlatform("tvos")]
	public abstract long MessagesReceived { get; }

	[UnsupportedOSPlatform("freebsd")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("osx")]
	[UnsupportedOSPlatform("tvos")]
	public abstract long MessagesSent { get; }

	public abstract long NeighborAdvertisementsReceived { get; }

	public abstract long NeighborAdvertisementsSent { get; }

	public abstract long NeighborSolicitsReceived { get; }

	public abstract long NeighborSolicitsSent { get; }

	public abstract long PacketTooBigMessagesReceived { get; }

	public abstract long PacketTooBigMessagesSent { get; }

	public abstract long ParameterProblemsReceived { get; }

	public abstract long ParameterProblemsSent { get; }

	public abstract long RedirectsReceived { get; }

	public abstract long RedirectsSent { get; }

	public abstract long RouterAdvertisementsReceived { get; }

	public abstract long RouterAdvertisementsSent { get; }

	public abstract long RouterSolicitsReceived { get; }

	public abstract long RouterSolicitsSent { get; }

	public abstract long TimeExceededMessagesReceived { get; }

	public abstract long TimeExceededMessagesSent { get; }
}
