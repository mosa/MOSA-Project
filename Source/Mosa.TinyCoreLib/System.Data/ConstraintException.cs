using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class ConstraintException : DataException
{
	public ConstraintException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ConstraintException(SerializationInfo info, StreamingContext context)
	{
	}

	public ConstraintException(string? s)
	{
	}

	public ConstraintException(string? message, Exception? innerException)
	{
	}
}
