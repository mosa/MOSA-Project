using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class VersionNotFoundException : DataException
{
	public VersionNotFoundException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected VersionNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public VersionNotFoundException(string? s)
	{
	}

	public VersionNotFoundException(string? message, Exception? innerException)
	{
	}
}
