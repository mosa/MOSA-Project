namespace System.Net.Sockets;

public struct SocketReceiveMessageFromResult
{
	public IPPacketInformation PacketInformation;

	public int ReceivedBytes;

	public EndPoint RemoteEndPoint;

	public SocketFlags SocketFlags;
}
