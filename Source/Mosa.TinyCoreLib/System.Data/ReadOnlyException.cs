using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class ReadOnlyException : DataException
{
	public ReadOnlyException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ReadOnlyException(SerializationInfo info, StreamingContext context)
	{
	}

	public ReadOnlyException(string? s)
	{
	}

	public ReadOnlyException(string? message, Exception? innerException)
	{
	}
}
