using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class DataException : SystemException
{
	public DataException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DataException(SerializationInfo info, StreamingContext context)
	{
	}

	public DataException(string? s)
	{
	}

	public DataException(string? s, Exception? innerException)
	{
	}
}
