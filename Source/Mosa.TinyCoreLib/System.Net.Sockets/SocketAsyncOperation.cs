namespace System.Net.Sockets;

public enum SocketAsyncOperation
{
	None,
	Accept,
	Connect,
	Disconnect,
	Receive,
	ReceiveFrom,
	ReceiveMessageFrom,
	Send,
	SendPackets,
	SendTo
}
