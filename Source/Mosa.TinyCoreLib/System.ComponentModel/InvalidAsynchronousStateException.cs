using System.Runtime.Serialization;

namespace System.ComponentModel;

public class InvalidAsynchronousStateException : ArgumentException
{
	public InvalidAsynchronousStateException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InvalidAsynchronousStateException(SerializationInfo info, StreamingContext context)
	{
	}

	public InvalidAsynchronousStateException(string? message)
	{
	}

	public InvalidAsynchronousStateException(string? message, Exception? innerException)
	{
	}
}
