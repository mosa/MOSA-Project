using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class InvalidTimeZoneException : Exception
{
	public InvalidTimeZoneException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InvalidTimeZoneException(SerializationInfo info, StreamingContext context)
	{
	}

	public InvalidTimeZoneException(string? message)
	{
	}

	public InvalidTimeZoneException(string? message, Exception? innerException)
	{
	}
}
