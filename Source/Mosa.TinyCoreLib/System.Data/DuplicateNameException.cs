using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class DuplicateNameException : DataException
{
	public DuplicateNameException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DuplicateNameException(SerializationInfo info, StreamingContext context)
	{
	}

	public DuplicateNameException(string? s)
	{
	}

	public DuplicateNameException(string? message, Exception? innerException)
	{
	}
}
