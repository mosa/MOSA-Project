using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class InvalidExpressionException : DataException
{
	public InvalidExpressionException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InvalidExpressionException(SerializationInfo info, StreamingContext context)
	{
	}

	public InvalidExpressionException(string? s)
	{
	}

	public InvalidExpressionException(string? message, Exception? innerException)
	{
	}
}
