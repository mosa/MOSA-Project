using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class SignedCms
{
	public X509Certificate2Collection Certificates
	{
		get
		{
			throw null;
		}
	}

	public ContentInfo ContentInfo
	{
		get
		{
			throw null;
		}
	}

	public bool Detached
	{
		get
		{
			throw null;
		}
	}

	public SignerInfoCollection SignerInfos
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

	public SignedCms()
	{
	}

	public SignedCms(ContentInfo contentInfo)
	{
	}

	public SignedCms(ContentInfo contentInfo, bool detached)
	{
	}

	public SignedCms(SubjectIdentifierType signerIdentifierType)
	{
	}

	public SignedCms(SubjectIdentifierType signerIdentifierType, ContentInfo contentInfo)
	{
	}

	public SignedCms(SubjectIdentifierType signerIdentifierType, ContentInfo contentInfo, bool detached)
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComputeSignature()
	{
	}

	public void ComputeSignature(CmsSigner signer)
	{
	}

	public void ComputeSignature(CmsSigner signer, bool silent)
	{
	}

	public void Decode(byte[] encodedMessage)
	{
	}

	public byte[] Encode()
	{
		throw null;
	}

	public void RemoveSignature(int index)
	{
	}

	public void RemoveSignature(SignerInfo signerInfo)
	{
	}

	public void AddCertificate(X509Certificate2 certificate)
	{
	}

	public void Decode(ReadOnlySpan<byte> encodedMessage)
	{
	}

	public void RemoveCertificate(X509Certificate2 certificate)
	{
	}
}
