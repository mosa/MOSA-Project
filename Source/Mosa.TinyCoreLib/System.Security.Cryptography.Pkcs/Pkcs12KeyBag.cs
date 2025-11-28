namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs12KeyBag : Pkcs12SafeBag
{
	public ReadOnlyMemory<byte> Pkcs8PrivateKey
	{
		get
		{
			throw null;
		}
	}

	public Pkcs12KeyBag(ReadOnlyMemory<byte> pkcs8PrivateKey, bool skipCopy = false)
		: base(null, default(ReadOnlyMemory<byte>))
	{
	}
}
