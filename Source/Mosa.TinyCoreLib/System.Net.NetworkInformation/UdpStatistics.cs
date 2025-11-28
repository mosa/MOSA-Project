namespace System.Net.NetworkInformation;

public abstract class UdpStatistics
{
	public abstract long DatagramsReceived { get; }

	public abstract long DatagramsSent { get; }

	public abstract long IncomingDatagramsDiscarded { get; }

	public abstract long IncomingDatagramsWithErrors { get; }

	public abstract int UdpListeners { get; }
}
