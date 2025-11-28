using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class CmsRecipient
{
	public X509Certificate2 Certificate
	{
		get
		{
			throw null;
		}
	}

	public SubjectIdentifierType RecipientIdentifierType
	{
		get
		{
			throw null;
		}
	}

	public RSAEncryptionPadding? RSAEncryptionPadding
	{
		get
		{
			throw null;
		}
	}

	public CmsRecipient(SubjectIdentifierType recipientIdentifierType, X509Certificate2 certificate)
	{
	}

	public CmsRecipient(X509Certificate2 certificate)
	{
	}

	public CmsRecipient(X509Certificate2 certificate, RSAEncryptionPadding rsaEncryptionPadding)
	{
	}

	public CmsRecipient(SubjectIdentifierType recipientIdentifierType, X509Certificate2 certificate, RSAEncryptionPadding rsaEncryptionPadding)
	{
	}
}
