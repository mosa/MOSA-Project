using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class MethodAccessException : MemberAccessException
{
	public MethodAccessException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected MethodAccessException(SerializationInfo info, StreamingContext context)
	{
	}

	public MethodAccessException(string? message)
	{
	}

	public MethodAccessException(string? message, Exception? inner)
	{
	}
}
