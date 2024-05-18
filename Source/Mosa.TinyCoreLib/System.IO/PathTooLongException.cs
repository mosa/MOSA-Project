using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.IO;

public class PathTooLongException : IOException
{
	public PathTooLongException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected PathTooLongException(SerializationInfo info, StreamingContext context)
	{
	}

	public PathTooLongException(string? message)
	{
	}

	public PathTooLongException(string? message, Exception? innerException)
	{
	}
}
