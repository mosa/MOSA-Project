namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs12SecretBag : Pkcs12SafeBag
{
	public ReadOnlyMemory<byte> SecretValue
	{
		get
		{
			throw null;
		}
	}

	internal Pkcs12SecretBag()
		: base(null, default(ReadOnlyMemory<byte>))
	{
	}

	public Oid GetSecretType()
	{
		throw null;
	}
}
