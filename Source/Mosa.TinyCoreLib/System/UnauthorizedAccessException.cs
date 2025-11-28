using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class UnauthorizedAccessException : SystemException
{
	public UnauthorizedAccessException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected UnauthorizedAccessException(SerializationInfo info, StreamingContext context)
	{
	}

	public UnauthorizedAccessException(string? message)
	{
	}

	public UnauthorizedAccessException(string? message, Exception? inner)
	{
	}
}
