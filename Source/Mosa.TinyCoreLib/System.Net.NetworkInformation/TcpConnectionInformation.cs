namespace System.Net.NetworkInformation;

public abstract class TcpConnectionInformation
{
	public abstract IPEndPoint LocalEndPoint { get; }

	public abstract IPEndPoint RemoteEndPoint { get; }

	public abstract TcpState State { get; }
}
