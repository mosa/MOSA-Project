namespace System.Security.Cryptography;

public sealed class AuthenticationTagMismatchException : CryptographicException
{
	public AuthenticationTagMismatchException()
	{
	}

	public AuthenticationTagMismatchException(string? message)
	{
	}

	public AuthenticationTagMismatchException(string? message, Exception? inner)
	{
	}
}
