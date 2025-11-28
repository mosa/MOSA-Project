using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Security.Authentication;

public class InvalidCredentialException : AuthenticationException
{
	public InvalidCredentialException()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected InvalidCredentialException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public InvalidCredentialException(string? message)
	{
	}

	public InvalidCredentialException(string? message, Exception? innerException)
	{
	}
}
