namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs12ShroudedKeyBag : Pkcs12SafeBag
{
	public ReadOnlyMemory<byte> EncryptedPkcs8PrivateKey
	{
		get
		{
			throw null;
		}
	}

	public Pkcs12ShroudedKeyBag(ReadOnlyMemory<byte> encryptedPkcs8PrivateKey, bool skipCopy = false)
		: base(null, default(ReadOnlyMemory<byte>))
	{
	}
}
