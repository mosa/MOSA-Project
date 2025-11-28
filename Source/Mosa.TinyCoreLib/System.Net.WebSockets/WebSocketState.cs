namespace System.Net.WebSockets;

public enum WebSocketState
{
	None,
	Connecting,
	Open,
	CloseSent,
	CloseReceived,
	Closed,
	Aborted
}
