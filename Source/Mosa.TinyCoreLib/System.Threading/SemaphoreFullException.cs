using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading;

public class SemaphoreFullException : SystemException
{
	public SemaphoreFullException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SemaphoreFullException(SerializationInfo info, StreamingContext context)
	{
	}

	public SemaphoreFullException(string? message)
	{
	}

	public SemaphoreFullException(string? message, Exception? innerException)
	{
	}
}
