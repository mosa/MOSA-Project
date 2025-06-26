using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class PlatformNotSupportedException : NotSupportedException
{
	public PlatformNotSupportedException()
		: base(Internal.Exceptions.PlatformNotSupportedException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected PlatformNotSupportedException(SerializationInfo info, StreamingContext context) {}

	public PlatformNotSupportedException(string? message)
		: base(message) {}

	public PlatformNotSupportedException(string? message, Exception? inner)
		: base(message, inner) {}
}
