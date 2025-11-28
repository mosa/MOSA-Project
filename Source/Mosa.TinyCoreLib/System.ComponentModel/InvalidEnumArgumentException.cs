using System.Runtime.Serialization;

namespace System.ComponentModel;

public class InvalidEnumArgumentException : ArgumentException
{
	public InvalidEnumArgumentException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InvalidEnumArgumentException(SerializationInfo info, StreamingContext context)
	{
	}

	public InvalidEnumArgumentException(string? message)
	{
	}

	public InvalidEnumArgumentException(string? message, Exception? innerException)
	{
	}

	public InvalidEnumArgumentException(string? argumentName, int invalidValue, Type enumClass)
	{
	}
}
