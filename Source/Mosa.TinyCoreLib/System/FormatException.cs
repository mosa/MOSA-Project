using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class FormatException : SystemException
{
	public FormatException()
		: base(Internal.Exceptions.FormatException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected FormatException(SerializationInfo info, StreamingContext context) {}

	public FormatException(string? message)
		: base(message) {}

	public FormatException(string? message, Exception? innerException)
		: base(message, innerException) {}
}
