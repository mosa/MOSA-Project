using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class NotSupportedException : SystemException
{
	public NotSupportedException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected NotSupportedException(SerializationInfo info, StreamingContext context)
	{
	}

	public NotSupportedException(string? message)
	{
	}

	public NotSupportedException(string? message, Exception? innerException)
	{
	}
}
