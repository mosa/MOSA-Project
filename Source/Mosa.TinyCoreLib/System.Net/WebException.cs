using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net;

public class WebException : InvalidOperationException, ISerializable
{
	public WebResponse? Response
	{
		get
		{
			throw null;
		}
	}

	public WebExceptionStatus Status
	{
		get
		{
			throw null;
		}
	}

	public WebException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected WebException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public WebException(string? message)
	{
	}

	public WebException(string? message, Exception? innerException)
	{
	}

	public WebException(string? message, Exception? innerException, WebExceptionStatus status, WebResponse? response)
	{
	}

	public WebException(string? message, WebExceptionStatus status)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
