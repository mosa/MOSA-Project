using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.IO;

public class EndOfStreamException : IOException
{
	public EndOfStreamException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected EndOfStreamException(SerializationInfo info, StreamingContext context)
	{
	}

	public EndOfStreamException(string? message)
	{
	}

	public EndOfStreamException(string? message, Exception? innerException)
	{
	}
}
