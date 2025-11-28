using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class ContextMarshalException : SystemException
{
	public ContextMarshalException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ContextMarshalException(SerializationInfo info, StreamingContext context)
	{
	}

	public ContextMarshalException(string? message)
	{
	}

	public ContextMarshalException(string? message, Exception? inner)
	{
	}
}
