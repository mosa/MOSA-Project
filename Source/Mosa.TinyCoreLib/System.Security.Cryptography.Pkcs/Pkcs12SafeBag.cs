namespace System.Security.Cryptography.Pkcs;

public abstract class Pkcs12SafeBag
{
	public CryptographicAttributeObjectCollection Attributes
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte> EncodedBagValue
	{
		get
		{
			throw null;
		}
	}

	protected Pkcs12SafeBag(string bagIdValue, ReadOnlyMemory<byte> encodedBagValue, bool skipCopy = false)
	{
	}

	public byte[] Encode()
	{
		throw null;
	}

	public Oid GetBagId()
	{
		throw null;
	}

	public bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
