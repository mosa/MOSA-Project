using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Diagnostics.Tracing;

public class EventSourceException : Exception
{
	public EventSourceException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected EventSourceException(SerializationInfo info, StreamingContext context)
	{
	}

	public EventSourceException(string? message)
	{
	}

	public EventSourceException(string? message, Exception? innerException)
	{
	}
}
