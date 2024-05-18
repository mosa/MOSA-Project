using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class DllNotFoundException : TypeLoadException
{
	public DllNotFoundException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DllNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public DllNotFoundException(string? message)
	{
	}

	public DllNotFoundException(string? message, Exception? inner)
	{
	}
}
