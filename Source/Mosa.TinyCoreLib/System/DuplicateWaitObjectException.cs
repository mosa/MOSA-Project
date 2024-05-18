using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class DuplicateWaitObjectException : ArgumentException
{
	public DuplicateWaitObjectException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DuplicateWaitObjectException(SerializationInfo info, StreamingContext context)
	{
	}

	public DuplicateWaitObjectException(string? parameterName)
	{
	}

	public DuplicateWaitObjectException(string? message, Exception? innerException)
	{
	}

	public DuplicateWaitObjectException(string? parameterName, string? message)
	{
	}
}
