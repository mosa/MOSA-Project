namespace System.Net.WebSockets;

public enum WebSocketError
{
	Success,
	InvalidMessageType,
	Faulted,
	NativeError,
	NotAWebSocket,
	UnsupportedVersion,
	UnsupportedProtocol,
	HeaderError,
	ConnectionClosedPrematurely,
	InvalidState
}
