using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class DSACng : DSA
{
	public CngKey Key
	{
		get
		{
			throw null;
		}
	}

	public override string? KeyExchangeAlgorithm
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

	public override string SignatureAlgorithm
	{
		get
		{
			throw null;
		}
	}

	[SupportedOSPlatform("windows")]
	public DSACng()
	{
	}

	[SupportedOSPlatform("windows")]
	public DSACng(int keySize)
	{
	}

	[SupportedOSPlatform("windows")]
	public DSACng(CngKey key)
	{
	}

	public override byte[] CreateSignature(byte[] rgbHash)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
	{
		throw null;
	}

	public override byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters)
	{
		throw null;
	}

	public override DSAParameters ExportParameters(bool includePrivateParameters)
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

	public override void ImportParameters(DSAParameters parameters)
	{
	}

	protected override bool TryCreateSignatureCore(ReadOnlySpan<byte> hash, Span<byte> destination, DSASignatureFormat signatureFormat, out int bytesWritten)
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

	public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
	{
		throw null;
	}

	protected override bool VerifySignatureCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, DSASignatureFormat signatureFormat)
	{
		throw null;
	}
}
