using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class DeletedRowInaccessibleException : DataException
{
	public DeletedRowInaccessibleException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DeletedRowInaccessibleException(SerializationInfo info, StreamingContext context)
	{
	}

	public DeletedRowInaccessibleException(string? s)
	{
	}

	public DeletedRowInaccessibleException(string? message, Exception? innerException)
	{
	}
}
