namespace System.Net.WebSockets;

public class WebSocketReceiveResult
{
	public WebSocketCloseStatus? CloseStatus
	{
		get
		{
			throw null;
		}
	}

	public string? CloseStatusDescription
	{
		get
		{
			throw null;
		}
	}

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

	public WebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage)
	{
	}

	public WebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage, WebSocketCloseStatus? closeStatus, string? closeStatusDescription)
	{
	}
}
