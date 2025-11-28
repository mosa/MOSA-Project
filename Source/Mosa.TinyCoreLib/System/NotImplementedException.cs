using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class NotImplementedException : SystemException
{
	public NotImplementedException()
		: base(Internal.Exceptions.NotImplementedException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected NotImplementedException(SerializationInfo info, StreamingContext context) {}

	public NotImplementedException(string? message)
		: base(message) {}

	public NotImplementedException(string? message, Exception? inner)
		: base(message, inner) {}
}
