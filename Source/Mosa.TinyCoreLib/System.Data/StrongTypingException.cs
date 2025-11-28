using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class StrongTypingException : DataException
{
	public StrongTypingException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected StrongTypingException(SerializationInfo info, StreamingContext context)
	{
	}

	public StrongTypingException(string? message)
	{
	}

	public StrongTypingException(string? s, Exception? innerException)
	{
	}
}
