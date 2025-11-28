using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading;

public class ThreadInterruptedException : SystemException
{
	public ThreadInterruptedException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ThreadInterruptedException(SerializationInfo info, StreamingContext context)
	{
	}

	public ThreadInterruptedException(string? message)
	{
	}

	public ThreadInterruptedException(string? message, Exception? innerException)
	{
	}
}
