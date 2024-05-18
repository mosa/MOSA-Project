using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class NullReferenceException : SystemException
{
	public NullReferenceException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected NullReferenceException(SerializationInfo info, StreamingContext context)
	{
	}

	public NullReferenceException(string? message)
	{
	}

	public NullReferenceException(string? message, Exception? innerException)
	{
	}
}
