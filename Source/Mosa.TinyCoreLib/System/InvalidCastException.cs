using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class InvalidCastException : SystemException
{
	public InvalidCastException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InvalidCastException(SerializationInfo info, StreamingContext context)
	{
	}

	public InvalidCastException(string? message)
	{
	}

	public InvalidCastException(string? message, Exception? innerException)
	{
	}

	public InvalidCastException(string? message, int errorCode)
	{
	}
}
