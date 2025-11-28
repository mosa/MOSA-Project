using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Security.Authentication;

public class AuthenticationException : SystemException
{
	public AuthenticationException()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected AuthenticationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public AuthenticationException(string? message)
	{
	}

	public AuthenticationException(string? message, Exception? innerException)
	{
	}
}
