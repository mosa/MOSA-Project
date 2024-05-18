using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class CannotUnloadAppDomainException : SystemException
{
	public CannotUnloadAppDomainException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected CannotUnloadAppDomainException(SerializationInfo info, StreamingContext context)
	{
	}

	public CannotUnloadAppDomainException(string? message)
	{
	}

	public CannotUnloadAppDomainException(string? message, Exception? innerException)
	{
	}
}
