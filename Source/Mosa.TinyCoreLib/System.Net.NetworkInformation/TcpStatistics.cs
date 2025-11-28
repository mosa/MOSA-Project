namespace System.Net.NetworkInformation;

public abstract class TcpStatistics
{
	public abstract long ConnectionsAccepted { get; }

	public abstract long ConnectionsInitiated { get; }

	public abstract long CumulativeConnections { get; }

	public abstract long CurrentConnections { get; }

	public abstract long ErrorsReceived { get; }

	public abstract long FailedConnectionAttempts { get; }

	public abstract long MaximumConnections { get; }

	public abstract long MaximumTransmissionTimeout { get; }

	public abstract long MinimumTransmissionTimeout { get; }

	public abstract long ResetConnections { get; }

	public abstract long ResetsSent { get; }

	public abstract long SegmentsReceived { get; }

	public abstract long SegmentsResent { get; }

	public abstract long SegmentsSent { get; }
}
