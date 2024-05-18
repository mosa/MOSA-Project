using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class RowNotInTableException : DataException
{
	public RowNotInTableException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected RowNotInTableException(SerializationInfo info, StreamingContext context)
	{
	}

	public RowNotInTableException(string? s)
	{
	}

	public RowNotInTableException(string? message, Exception? innerException)
	{
	}
}
