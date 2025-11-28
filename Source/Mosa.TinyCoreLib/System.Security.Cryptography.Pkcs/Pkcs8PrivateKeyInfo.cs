namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs8PrivateKeyInfo
{
	public Oid AlgorithmId
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte>? AlgorithmParameters
	{
		get
		{
			throw null;
		}
	}

	public CryptographicAttributeObjectCollection Attributes
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte> PrivateKeyBytes
	{
		get
		{
			throw null;
		}
	}

	public Pkcs8PrivateKeyInfo(Oid algorithmId, ReadOnlyMemory<byte>? algorithmParameters, ReadOnlyMemory<byte> privateKey, bool skipCopies = false)
	{
	}

	public static Pkcs8PrivateKeyInfo Create(AsymmetricAlgorithm privateKey)
	{
		throw null;
	}

	public static Pkcs8PrivateKeyInfo Decode(ReadOnlyMemory<byte> source, out int bytesRead, bool skipCopy = false)
	{
		throw null;
	}

	public static Pkcs8PrivateKeyInfo DecryptAndDecode(ReadOnlySpan<byte> passwordBytes, ReadOnlyMemory<byte> source, out int bytesRead)
	{
		throw null;
	}

	public static Pkcs8PrivateKeyInfo DecryptAndDecode(ReadOnlySpan<char> password, ReadOnlyMemory<byte> source, out int bytesRead)
	{
		throw null;
	}

	public byte[] Encode()
	{
		throw null;
	}

	public byte[] Encrypt(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
	{
		throw null;
	}

	public byte[] Encrypt(ReadOnlySpan<char> password, PbeParameters pbeParameters)
	{
		throw null;
	}

	public bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryEncrypt(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryEncrypt(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
