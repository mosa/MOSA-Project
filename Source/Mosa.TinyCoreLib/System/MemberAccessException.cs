using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class MemberAccessException : SystemException
{
	public MemberAccessException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected MemberAccessException(SerializationInfo info, StreamingContext context)
	{
	}

	public MemberAccessException(string? message)
	{
	}

	public MemberAccessException(string? message, Exception? inner)
	{
	}
}
