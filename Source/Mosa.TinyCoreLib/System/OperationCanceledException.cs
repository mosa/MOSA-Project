using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;

namespace System;

public class OperationCanceledException : SystemException
{
	public CancellationToken CancellationToken
	{
		get
		{
			throw null;
		}
	}

	public OperationCanceledException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected OperationCanceledException(SerializationInfo info, StreamingContext context)
	{
	}

	public OperationCanceledException(string? message)
	{
	}

	public OperationCanceledException(string? message, Exception? innerException)
	{
	}

	public OperationCanceledException(string? message, Exception? innerException, CancellationToken token)
	{
	}

	public OperationCanceledException(string? message, CancellationToken token)
	{
	}

	public OperationCanceledException(CancellationToken token)
	{
	}
}
