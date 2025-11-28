using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.IO;

public class DirectoryNotFoundException : IOException
{
	public DirectoryNotFoundException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DirectoryNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public DirectoryNotFoundException(string? message)
	{
	}

	public DirectoryNotFoundException(string? message, Exception? innerException)
	{
	}
}
