using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading;

public class ThreadStateException : SystemException
{
	public ThreadStateException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ThreadStateException(SerializationInfo info, StreamingContext context)
	{
	}

	public ThreadStateException(string? message)
	{
	}

	public ThreadStateException(string? message, Exception? innerException)
	{
	}
}
