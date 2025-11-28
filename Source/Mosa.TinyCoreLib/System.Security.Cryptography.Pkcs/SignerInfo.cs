using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class SignerInfo
{
	public X509Certificate2? Certificate
	{
		get
		{
			throw null;
		}
	}

	public SignerInfoCollection CounterSignerInfos
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
	}

	public CryptographicAttributeObjectCollection SignedAttributes
	{
		get
		{
			throw null;
		}
	}

	public SubjectIdentifier SignerIdentifier
	{
		get
		{
			throw null;
		}
	}

	public CryptographicAttributeObjectCollection UnsignedAttributes
	{
		get
		{
			throw null;
		}
	}

	public int Version
	{
		get
		{
			throw null;
		}
	}

	public Oid SignatureAlgorithm
	{
		get
		{
			throw null;
		}
	}

	internal SignerInfo()
	{
	}

	public void CheckHash()
	{
	}

	public void CheckSignature(bool verifySignatureOnly)
	{
	}

	public void CheckSignature(X509Certificate2Collection extraStore, bool verifySignatureOnly)
	{
	}

	[Obsolete("ComputeCounterSignature without specifying a CmsSigner is obsolete and is not supported. Use the overload that accepts a CmsSigner.", DiagnosticId = "SYSLIB0035", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComputeCounterSignature()
	{
	}

	public void ComputeCounterSignature(CmsSigner signer)
	{
	}

	public void RemoveCounterSignature(int index)
	{
	}

	public void RemoveCounterSignature(SignerInfo counterSignerInfo)
	{
	}

	public byte[] GetSignature()
	{
		throw null;
	}

	public void AddUnsignedAttribute(AsnEncodedData unsignedAttribute)
	{
	}

	public void RemoveUnsignedAttribute(AsnEncodedData unsignedAttribute)
	{
	}
}
