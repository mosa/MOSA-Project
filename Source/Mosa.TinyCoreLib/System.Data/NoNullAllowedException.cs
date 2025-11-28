using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class NoNullAllowedException : DataException
{
	public NoNullAllowedException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected NoNullAllowedException(SerializationInfo info, StreamingContext context)
	{
	}

	public NoNullAllowedException(string? s)
	{
	}

	public NoNullAllowedException(string? message, Exception? innerException)
	{
	}
}
