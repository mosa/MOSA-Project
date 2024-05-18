namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs12SafeContentsBag : Pkcs12SafeBag
{
	public Pkcs12SafeContents? SafeContents
	{
		get
		{
			throw null;
		}
	}

	internal Pkcs12SafeContentsBag()
		: base(null, default(ReadOnlyMemory<byte>))
	{
	}
}
