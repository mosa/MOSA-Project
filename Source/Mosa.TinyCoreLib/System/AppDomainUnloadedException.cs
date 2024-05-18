using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class AppDomainUnloadedException : SystemException
{
	public AppDomainUnloadedException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected AppDomainUnloadedException(SerializationInfo info, StreamingContext context)
	{
	}

	public AppDomainUnloadedException(string? message)
	{
	}

	public AppDomainUnloadedException(string? message, Exception? innerException)
	{
	}
}
