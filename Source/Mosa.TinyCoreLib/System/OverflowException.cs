using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class OverflowException : ArithmeticException
{
	public OverflowException()
		: base(Internal.Exceptions.OverflowException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected OverflowException(SerializationInfo info, StreamingContext context) {}

	public OverflowException(string? message)
		: base(message) {}

	public OverflowException(string? message, Exception? innerException)
		: base(message, innerException) {}
}
