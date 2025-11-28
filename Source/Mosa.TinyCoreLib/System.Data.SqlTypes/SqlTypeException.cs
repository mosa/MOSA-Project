using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data.SqlTypes;

public class SqlTypeException : SystemException
{
	public SqlTypeException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SqlTypeException(SerializationInfo si, StreamingContext sc)
	{
	}

	public SqlTypeException(string? message)
	{
	}

	public SqlTypeException(string? message, Exception? e)
	{
	}
}
