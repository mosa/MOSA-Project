using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class ArithmeticException : SystemException
{
	public ArithmeticException()
		: base(Internal.Exceptions.ArithmeticException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ArithmeticException(SerializationInfo info, StreamingContext context) {}

	public ArithmeticException(string? message)
		: base(message) {}

	public ArithmeticException(string? message, Exception? innerException)
		: base(message, innerException) {}
}
