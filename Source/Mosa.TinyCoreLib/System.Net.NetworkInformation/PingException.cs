using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net.NetworkInformation;

public class PingException : InvalidOperationException
{
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected PingException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public PingException(string? message)
	{
	}

	public PingException(string? message, Exception? innerException)
	{
	}
}
