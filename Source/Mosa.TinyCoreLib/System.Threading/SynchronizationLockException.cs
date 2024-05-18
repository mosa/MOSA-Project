using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading;

public class SynchronizationLockException : SystemException
{
	public SynchronizationLockException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SynchronizationLockException(SerializationInfo info, StreamingContext context)
	{
	}

	public SynchronizationLockException(string? message)
	{
	}

	public SynchronizationLockException(string? message, Exception? innerException)
	{
	}
}
