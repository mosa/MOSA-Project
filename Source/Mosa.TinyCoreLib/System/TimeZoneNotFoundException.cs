using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class TimeZoneNotFoundException : Exception
{
	public TimeZoneNotFoundException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TimeZoneNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public TimeZoneNotFoundException(string? message)
	{
	}

	public TimeZoneNotFoundException(string? message, Exception? innerException)
	{
	}
}
