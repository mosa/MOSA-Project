using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class RSACng : RSA
{
	public CngKey Key
	{
		get
		{
			throw null;
		}
	}

	public override KeySizes[] LegalKeySizes
	{
		get
		{
			throw null;
		}
	}

	[SupportedOSPlatform("windows")]
	public RSACng()
	{
	}

	[SupportedOSPlatform("windows")]
	public RSACng(int keySize)
	{
	}

	[SupportedOSPlatform("windows")]
	public RSACng(CngKey key)
	{
	}

	public override byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
	{
		throw null;
	}

	public override byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
	{
		throw null;
	}

	public override byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters)
	{
		throw null;
	}

	public override RSAParameters ExportParameters(bool includePrivateParameters)
	{
		throw null;
	}

	public override void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportParameters(RSAParameters parameters)
	{
	}

	public override void ImportPkcs8PrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public override bool TryDecrypt(ReadOnlySpan<byte> data, Span<byte> destination, RSAEncryptionPadding padding, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryEncrypt(ReadOnlySpan<byte> data, Span<byte> destination, RSAEncryptionPadding padding, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public override bool TrySignHash(ReadOnlySpan<byte> hash, Span<byte> destination, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding, out int bytesWritten)
	{
		throw null;
	}

	public override bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public override bool VerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}
}
