using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class SystemException : Exception
{
	public SystemException()
		: base(Internal.Exceptions.SystemException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SystemException(SerializationInfo info, StreamingContext context) {}

	public SystemException(string? message)
		: base(message) {}

	public SystemException(string? message, Exception? innerException)
		: base(message, innerException) {}
}
