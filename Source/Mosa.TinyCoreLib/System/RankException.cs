using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class RankException : SystemException
{
	public RankException()
		: base(Internal.Exceptions.RankException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected RankException(SerializationInfo info, StreamingContext context) {}

	public RankException(string? message)
		: base(message) {}

	public RankException(string? message, Exception? innerException)
		: base(message, innerException) {}
}
