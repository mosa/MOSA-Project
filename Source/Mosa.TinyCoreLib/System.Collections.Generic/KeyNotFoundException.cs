using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Collections.Generic;

public class KeyNotFoundException : SystemException
{
	public KeyNotFoundException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected KeyNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public KeyNotFoundException(string? message)
	{
	}

	public KeyNotFoundException(string? message, Exception? innerException)
	{
	}
}
