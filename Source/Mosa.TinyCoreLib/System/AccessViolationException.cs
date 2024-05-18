using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class AccessViolationException : SystemException
{
	public AccessViolationException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected AccessViolationException(SerializationInfo info, StreamingContext context)
	{
	}

	public AccessViolationException(string? message)
	{
	}

	public AccessViolationException(string? message, Exception? innerException)
	{
	}
}
