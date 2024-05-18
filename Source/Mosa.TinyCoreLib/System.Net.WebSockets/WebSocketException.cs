using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net.WebSockets;

public sealed class WebSocketException : Win32Exception
{
	public override int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public WebSocketError WebSocketErrorCode
	{
		get
		{
			throw null;
		}
	}

	public WebSocketException()
	{
	}

	public WebSocketException(int nativeError)
	{
	}

	public WebSocketException(int nativeError, Exception? innerException)
	{
	}

	public WebSocketException(int nativeError, string? message)
	{
	}

	public WebSocketException(WebSocketError error)
	{
	}

	public WebSocketException(WebSocketError error, Exception? innerException)
	{
	}

	public WebSocketException(WebSocketError error, int nativeError)
	{
	}

	public WebSocketException(WebSocketError error, int nativeError, Exception? innerException)
	{
	}

	public WebSocketException(WebSocketError error, int nativeError, string? message)
	{
	}

	public WebSocketException(WebSocketError error, int nativeError, string? message, Exception? innerException)
	{
	}

	public WebSocketException(WebSocketError error, string? message)
	{
	}

	public WebSocketException(WebSocketError error, string? message, Exception? innerException)
	{
	}

	public WebSocketException(string? message)
	{
	}

	public WebSocketException(string? message, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
