namespace System.Net.WebSockets;

[Flags]
public enum WebSocketMessageFlags
{
	None = 0,
	EndOfMessage = 1,
	DisableCompression = 2
}
