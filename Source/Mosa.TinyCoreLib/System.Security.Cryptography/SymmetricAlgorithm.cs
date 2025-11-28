using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public abstract class SymmetricAlgorithm : IDisposable
{
	protected int BlockSizeValue;

	protected int FeedbackSizeValue;

	protected byte[]? IVValue;

	protected int KeySizeValue;

	protected byte[]? KeyValue;

	[MaybeNull]
	protected KeySizes[] LegalBlockSizesValue;

	[MaybeNull]
	protected KeySizes[] LegalKeySizesValue;

	protected CipherMode ModeValue;

	protected PaddingMode PaddingValue;

	public virtual int BlockSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual int FeedbackSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual byte[] IV
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual byte[] Key
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual int KeySize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual KeySizes[] LegalBlockSizes
	{
		get
		{
			throw null;
		}
	}

	public virtual KeySizes[] LegalKeySizes
	{
		get
		{
			throw null;
		}
	}

	public virtual CipherMode Mode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual PaddingMode Padding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public void Clear()
	{
	}

	[Obsolete("The default implementation of this cryptography algorithm is not supported.", DiagnosticId = "SYSLIB0007", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static SymmetricAlgorithm Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static SymmetricAlgorithm? Create(string algName)
	{
		throw null;
	}

	public virtual ICryptoTransform CreateDecryptor()
	{
		throw null;
	}

	public abstract ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV);

	public virtual ICryptoTransform CreateEncryptor()
	{
		throw null;
	}

	public abstract ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV);

	public byte[] DecryptCbc(byte[] ciphertext, byte[] iv, PaddingMode paddingMode = PaddingMode.PKCS7)
	{
		throw null;
	}

	public byte[] DecryptCbc(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, PaddingMode paddingMode = PaddingMode.PKCS7)
	{
		throw null;
	}

	public int DecryptCbc(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode = PaddingMode.PKCS7)
	{
		throw null;
	}

	public byte[] DecryptCfb(byte[] ciphertext, byte[] iv, PaddingMode paddingMode = PaddingMode.None, int feedbackSizeInBits = 8)
	{
		throw null;
	}

	public byte[] DecryptCfb(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, PaddingMode paddingMode = PaddingMode.None, int feedbackSizeInBits = 8)
	{
		throw null;
	}

	public int DecryptCfb(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode = PaddingMode.None, int feedbackSizeInBits = 8)
	{
		throw null;
	}

	public byte[] DecryptEcb(byte[] ciphertext, PaddingMode paddingMode)
	{
		throw null;
	}

	public byte[] DecryptEcb(ReadOnlySpan<byte> ciphertext, PaddingMode paddingMode)
	{
		throw null;
	}

	public int DecryptEcb(ReadOnlySpan<byte> ciphertext, Span<byte> destination, PaddingMode paddingMode)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public byte[] EncryptCbc(byte[] plaintext, byte[] iv, PaddingMode paddingMode = PaddingMode.PKCS7)
	{
		throw null;
	}

	public byte[] EncryptCbc(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, PaddingMode paddingMode = PaddingMode.PKCS7)
	{
		throw null;
	}

	public int EncryptCbc(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode = PaddingMode.PKCS7)
	{
		throw null;
	}

	public byte[] EncryptCfb(byte[] plaintext, byte[] iv, PaddingMode paddingMode = PaddingMode.None, int feedbackSizeInBits = 8)
	{
		throw null;
	}

	public byte[] EncryptCfb(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, PaddingMode paddingMode = PaddingMode.None, int feedbackSizeInBits = 8)
	{
		throw null;
	}

	public int EncryptCfb(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode = PaddingMode.None, int feedbackSizeInBits = 8)
	{
		throw null;
	}

	public byte[] EncryptEcb(byte[] plaintext, PaddingMode paddingMode)
	{
		throw null;
	}

	public byte[] EncryptEcb(ReadOnlySpan<byte> plaintext, PaddingMode paddingMode)
	{
		throw null;
	}

	public int EncryptEcb(ReadOnlySpan<byte> plaintext, Span<byte> destination, PaddingMode paddingMode)
	{
		throw null;
	}

	public abstract void GenerateIV();

	public abstract void GenerateKey();

	public int GetCiphertextLengthCbc(int plaintextLength, PaddingMode paddingMode = PaddingMode.PKCS7)
	{
		throw null;
	}

	public int GetCiphertextLengthCfb(int plaintextLength, PaddingMode paddingMode = PaddingMode.None, int feedbackSizeInBits = 8)
	{
		throw null;
	}

	public int GetCiphertextLengthEcb(int plaintextLength, PaddingMode paddingMode)
	{
		throw null;
	}

	public bool TryDecryptCbc(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, Span<byte> destination, out int bytesWritten, PaddingMode paddingMode = PaddingMode.PKCS7)
	{
		throw null;
	}

	protected virtual bool TryDecryptCbcCore(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}

	public bool TryDecryptCfb(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, Span<byte> destination, out int bytesWritten, PaddingMode paddingMode = PaddingMode.None, int feedbackSizeInBits = 8)
	{
		throw null;
	}

	protected virtual bool TryDecryptCfbCore(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode, int feedbackSizeInBits, out int bytesWritten)
	{
		throw null;
	}

	public bool TryDecryptEcb(ReadOnlySpan<byte> ciphertext, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}

	protected virtual bool TryDecryptEcbCore(ReadOnlySpan<byte> ciphertext, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}

	public bool TryEncryptCbc(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, Span<byte> destination, out int bytesWritten, PaddingMode paddingMode = PaddingMode.PKCS7)
	{
		throw null;
	}

	protected virtual bool TryEncryptCbcCore(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}

	public bool TryEncryptCfb(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, Span<byte> destination, out int bytesWritten, PaddingMode paddingMode = PaddingMode.None, int feedbackSizeInBits = 8)
	{
		throw null;
	}

	protected virtual bool TryEncryptCfbCore(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> iv, Span<byte> destination, PaddingMode paddingMode, int feedbackSizeInBits, out int bytesWritten)
	{
		throw null;
	}

	public bool TryEncryptEcb(ReadOnlySpan<byte> plaintext, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}

	protected virtual bool TryEncryptEcbCore(ReadOnlySpan<byte> plaintext, Span<byte> destination, PaddingMode paddingMode, out int bytesWritten)
	{
		throw null;
	}

	public bool ValidKeySize(int bitLength)
	{
		throw null;
	}
}
