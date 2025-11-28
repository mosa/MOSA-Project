using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public abstract class IcmpV4Statistics
{
	public abstract long AddressMaskRepliesReceived { get; }

	public abstract long AddressMaskRepliesSent { get; }

	public abstract long AddressMaskRequestsReceived { get; }

	public abstract long AddressMaskRequestsSent { get; }

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

	public abstract long ParameterProblemsReceived { get; }

	public abstract long ParameterProblemsSent { get; }

	public abstract long RedirectsReceived { get; }

	public abstract long RedirectsSent { get; }

	public abstract long SourceQuenchesReceived { get; }

	public abstract long SourceQuenchesSent { get; }

	public abstract long TimeExceededMessagesReceived { get; }

	public abstract long TimeExceededMessagesSent { get; }

	public abstract long TimestampRepliesReceived { get; }

	public abstract long TimestampRepliesSent { get; }

	public abstract long TimestampRequestsReceived { get; }

	public abstract long TimestampRequestsSent { get; }
}
