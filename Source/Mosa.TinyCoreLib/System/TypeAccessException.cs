using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class TypeAccessException : TypeLoadException
{
	public TypeAccessException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TypeAccessException(SerializationInfo info, StreamingContext context)
	{
	}

	public TypeAccessException(string? message)
	{
	}

	public TypeAccessException(string? message, Exception? inner)
	{
	}
}
