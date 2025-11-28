using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class EnvelopedCms
{
	public X509Certificate2Collection Certificates
	{
		get
		{
			throw null;
		}
	}

	public AlgorithmIdentifier ContentEncryptionAlgorithm
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

	public RecipientInfoCollection RecipientInfos
	{
		get
		{
			throw null;
		}
	}

	public CryptographicAttributeObjectCollection UnprotectedAttributes
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

	public EnvelopedCms()
	{
	}

	public EnvelopedCms(ContentInfo contentInfo)
	{
	}

	public EnvelopedCms(ContentInfo contentInfo, AlgorithmIdentifier encryptionAlgorithm)
	{
	}

	public void Decode(byte[] encodedMessage)
	{
	}

	public void Decrypt()
	{
	}

	public void Decrypt(RecipientInfo recipientInfo)
	{
	}

	public void Decrypt(RecipientInfo recipientInfo, X509Certificate2Collection extraStore)
	{
	}

	public void Decrypt(X509Certificate2Collection extraStore)
	{
	}

	public byte[] Encode()
	{
		throw null;
	}

	public void Encrypt(CmsRecipient recipient)
	{
	}

	public void Encrypt(CmsRecipientCollection recipients)
	{
	}

	public void Decode(ReadOnlySpan<byte> encodedMessage)
	{
	}

	public void Decrypt(RecipientInfo recipientInfo, AsymmetricAlgorithm? privateKey)
	{
	}
}
