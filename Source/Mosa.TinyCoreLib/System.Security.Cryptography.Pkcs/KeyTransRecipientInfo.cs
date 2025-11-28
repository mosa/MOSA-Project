namespace System.Security.Cryptography.Pkcs;

public sealed class KeyTransRecipientInfo : RecipientInfo
{
	public override byte[] EncryptedKey
	{
		get
		{
			throw null;
		}
	}

	public override AlgorithmIdentifier KeyEncryptionAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public override SubjectIdentifier RecipientIdentifier
	{
		get
		{
			throw null;
		}
	}

	public override int Version
	{
		get
		{
			throw null;
		}
	}

	internal KeyTransRecipientInfo()
	{
	}
}
