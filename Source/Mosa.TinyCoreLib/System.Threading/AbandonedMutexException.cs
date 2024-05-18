using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading;

public class AbandonedMutexException : SystemException
{
	public Mutex? Mutex
	{
		get
		{
			throw null;
		}
	}

	public int MutexIndex
	{
		get
		{
			throw null;
		}
	}

	public AbandonedMutexException()
	{
	}

	public AbandonedMutexException(int location, WaitHandle? handle)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected AbandonedMutexException(SerializationInfo info, StreamingContext context)
	{
	}

	public AbandonedMutexException(string? message)
	{
	}

	public AbandonedMutexException(string? message, Exception? inner)
	{
	}

	public AbandonedMutexException(string? message, Exception? inner, int location, WaitHandle? handle)
	{
	}

	public AbandonedMutexException(string? message, int location, WaitHandle? handle)
	{
	}
}
