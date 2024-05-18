namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs12Builder
{
	public bool IsSealed
	{
		get
		{
			throw null;
		}
	}

	public void AddSafeContentsEncrypted(Pkcs12SafeContents safeContents, byte[]? passwordBytes, PbeParameters pbeParameters)
	{
	}

	public void AddSafeContentsEncrypted(Pkcs12SafeContents safeContents, ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
	{
	}

	public void AddSafeContentsEncrypted(Pkcs12SafeContents safeContents, ReadOnlySpan<char> password, PbeParameters pbeParameters)
	{
	}

	public void AddSafeContentsEncrypted(Pkcs12SafeContents safeContents, string? password, PbeParameters pbeParameters)
	{
	}

	public void AddSafeContentsUnencrypted(Pkcs12SafeContents safeContents)
	{
	}

	public byte[] Encode()
	{
		throw null;
	}

	public void SealWithMac(ReadOnlySpan<char> password, HashAlgorithmName hashAlgorithm, int iterationCount)
	{
	}

	public void SealWithMac(string? password, HashAlgorithmName hashAlgorithm, int iterationCount)
	{
	}

	public void SealWithoutIntegrity()
	{
	}

	public bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
