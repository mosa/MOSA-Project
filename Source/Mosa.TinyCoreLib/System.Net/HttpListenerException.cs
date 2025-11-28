using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net;

public class HttpListenerException : Win32Exception
{
	public override int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public HttpListenerException()
	{
	}

	public HttpListenerException(int errorCode)
	{
	}

	public HttpListenerException(int errorCode, string message)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected HttpListenerException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
