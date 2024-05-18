namespace System.Net.WebSockets;

public readonly struct ValueWebSocketReceiveResult
{
	private readonly int _dummyPrimitive;

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool EndOfMessage
	{
		get
		{
			throw null;
		}
	}

	public WebSocketMessageType MessageType
	{
		get
		{
			throw null;
		}
	}

	public ValueWebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage)
	{
		throw null;
	}
}
