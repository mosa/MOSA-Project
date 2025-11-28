using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net.Sockets;

public class SocketException : Win32Exception
{
	public override int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public SocketError SocketErrorCode
	{
		get
		{
			throw null;
		}
	}

	public SocketException()
	{
	}

	public SocketException(int errorCode)
	{
	}

	public SocketException(int errorCode, string? message)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SocketException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
