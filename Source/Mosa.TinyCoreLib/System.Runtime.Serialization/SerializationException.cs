using System.ComponentModel;

namespace System.Runtime.Serialization;

public class SerializationException : SystemException
{
	public SerializationException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SerializationException(SerializationInfo info, StreamingContext context)
	{
	}

	public SerializationException(string? message)
	{
	}

	public SerializationException(string? message, Exception? innerException)
	{
	}
}
