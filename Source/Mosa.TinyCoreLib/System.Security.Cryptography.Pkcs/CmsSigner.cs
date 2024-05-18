using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class CmsSigner
{
	public X509Certificate2? Certificate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509Certificate2Collection Certificates
	{
		get
		{
			throw null;
		}
	}

	public Oid DigestAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509IncludeOption IncludeOption
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CryptographicAttributeObjectCollection SignedAttributes
	{
		get
		{
			throw null;
		}
	}

	public SubjectIdentifierType SignerIdentifierType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CryptographicAttributeObjectCollection UnsignedAttributes
	{
		get
		{
			throw null;
		}
	}

	public AsymmetricAlgorithm? PrivateKey
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RSASignaturePadding? SignaturePadding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CmsSigner()
	{
	}

	[Obsolete("CmsSigner(CspParameters) is obsolete and is not supported. Use an alternative constructor instead.", DiagnosticId = "SYSLIB0034", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public CmsSigner(CspParameters parameters)
	{
	}

	public CmsSigner(SubjectIdentifierType signerIdentifierType)
	{
	}

	public CmsSigner(SubjectIdentifierType signerIdentifierType, X509Certificate2? certificate)
	{
	}

	public CmsSigner(X509Certificate2? certificate)
	{
	}

	public CmsSigner(SubjectIdentifierType signerIdentifierType, X509Certificate2? certificate, AsymmetricAlgorithm? privateKey)
	{
	}

	public CmsSigner(SubjectIdentifierType signerIdentifierType, X509Certificate2? certificate, RSA? privateKey, RSASignaturePadding? signaturePadding)
	{
	}
}
