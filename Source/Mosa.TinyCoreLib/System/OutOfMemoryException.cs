using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class OutOfMemoryException : SystemException
{
	public OutOfMemoryException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected OutOfMemoryException(SerializationInfo info, StreamingContext context)
	{
	}

	public OutOfMemoryException(string? message)
	{
	}

	public OutOfMemoryException(string? message, Exception? innerException)
	{
	}
}
