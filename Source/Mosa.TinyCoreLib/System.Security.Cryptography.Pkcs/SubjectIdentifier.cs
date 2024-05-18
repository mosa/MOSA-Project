using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class SubjectIdentifier
{
	public SubjectIdentifierType Type
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
	}

	internal SubjectIdentifier()
	{
	}

	public bool MatchesCertificate(X509Certificate2 certificate)
	{
		throw null;
	}
}
