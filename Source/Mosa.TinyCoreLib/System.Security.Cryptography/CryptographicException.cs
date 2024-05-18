using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Security.Cryptography;

public class CryptographicException : SystemException
{
	public CryptographicException()
	{
	}

	public CryptographicException(int hr)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected CryptographicException(SerializationInfo info, StreamingContext context)
	{
	}

	public CryptographicException(string? message)
	{
	}

	public CryptographicException(string? message, Exception? inner)
	{
	}

	public CryptographicException([StringSyntax("CompositeFormat")] string format, string? insert)
	{
	}
}
