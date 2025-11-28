namespace System.Security.Cryptography.Pkcs;

public abstract class RecipientInfo
{
	public abstract byte[] EncryptedKey { get; }

	public abstract AlgorithmIdentifier KeyEncryptionAlgorithm { get; }

	public abstract SubjectIdentifier RecipientIdentifier { get; }

	public RecipientInfoType Type
	{
		get
		{
			throw null;
		}
	}

	public abstract int Version { get; }

	internal RecipientInfo()
	{
	}
}
