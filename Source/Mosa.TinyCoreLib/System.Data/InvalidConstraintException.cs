using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class InvalidConstraintException : DataException
{
	public InvalidConstraintException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InvalidConstraintException(SerializationInfo info, StreamingContext context)
	{
	}

	public InvalidConstraintException(string? s)
	{
	}

	public InvalidConstraintException(string? message, Exception? innerException)
	{
	}
}
