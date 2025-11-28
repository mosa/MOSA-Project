using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading;

public class LockRecursionException : Exception
{
	public LockRecursionException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected LockRecursionException(SerializationInfo info, StreamingContext context)
	{
	}

	public LockRecursionException(string? message)
	{
	}

	public LockRecursionException(string? message, Exception? innerException)
	{
	}
}
