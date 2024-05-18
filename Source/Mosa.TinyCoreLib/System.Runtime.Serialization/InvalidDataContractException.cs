using System.ComponentModel;

namespace System.Runtime.Serialization;

public class InvalidDataContractException : Exception
{
	public InvalidDataContractException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected InvalidDataContractException(SerializationInfo info, StreamingContext context)
	{
	}

	public InvalidDataContractException(string? message)
	{
	}

	public InvalidDataContractException(string? message, Exception? innerException)
	{
	}
}
