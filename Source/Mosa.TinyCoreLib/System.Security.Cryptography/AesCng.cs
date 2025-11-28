using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public sealed class AesCng : Aes
{
	public override byte[] Key
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override int KeySize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[SupportedOSPlatform("windows")]
	public AesCng()
	{
	}

	[SupportedOSPlatform("windows")]
	public AesCng(string keyName)
	{
	}

	[SupportedOSPlatform("windows")]
	public AesCng(string keyName, CngProvider provider)
	{
	}

	[SupportedOSPlatform("windows")]
	public AesCng(string keyName, CngProvider provider, CngKeyOpenOptions openOptions)
	{
	}

	public override ICryptoTransform CreateDecryptor()
	{
		throw null;
	}

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
	{
		throw null;
	}

	public override ICryptoTransform CreateEncryptor()
	{
		throw null;
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override void GenerateIV()
	{
	}

	public override void GenerateKey()
	{
	}

	protected override bool TryDecryptCbcCore(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}

	protected override bool TryDecryptCfbCore(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode, int feedbackSizeInBits, out int bytesWritten)
	{
		throw null;
	}

	protected override bool TryDecryptEcbCore(ReadOnlySpan<byte> ciphertext, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}

	protected override bool TryEncryptCbcCore(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}

	protected override bool TryEncryptCfbCore(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode, int feedbackSizeInBits, out int bytesWritten)
	{
		throw null;
	}

	protected override bool TryEncryptEcbCore(ReadOnlySpan<byte> plaintext, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}
}
