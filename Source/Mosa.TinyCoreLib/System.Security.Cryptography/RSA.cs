using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public abstract class RSA : AsymmetricAlgorithm
{
	public override string? KeyExchangeAlgorithm
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

	[UnsupportedOSPlatform("browser")]
	public new static RSA Create()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static RSA Create(int keySizeInBits)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static RSA Create(RSAParameters parameters)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static RSA? Create(string algName)
	{
		throw null;
	}

	public virtual byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
	{
		throw null;
	}

	public byte[] Decrypt(ReadOnlySpan<byte> data, RSAEncryptionPadding padding)
	{
		throw null;
	}

	public int Decrypt(ReadOnlySpan<byte> data, Span<byte> destination, RSAEncryptionPadding padding)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("RSA.EncryptValue and DecryptValue are not supported and throw NotSupportedException. Use RSA.Encrypt and RSA.Decrypt instead.", DiagnosticId = "SYSLIB0048", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual byte[] DecryptValue(byte[] rgb)
	{
		throw null;
	}

	public virtual byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
	{
		throw null;
	}

	public byte[] Encrypt(ReadOnlySpan<byte> data, RSAEncryptionPadding padding)
	{
		throw null;
	}

	public int Encrypt(ReadOnlySpan<byte> data, Span<byte> destination, RSAEncryptionPadding padding)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("RSA.EncryptValue and DecryptValue are not supported and throw NotSupportedException. Use RSA.Encrypt and RSA.Decrypt instead.", DiagnosticId = "SYSLIB0048", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual byte[] EncryptValue(byte[] rgb)
	{
		throw null;
	}

	public abstract RSAParameters ExportParameters(bool includePrivateParameters);

	public virtual byte[] ExportRSAPrivateKey()
	{
		throw null;
	}

	public string ExportRSAPrivateKeyPem()
	{
		throw null;
	}

	public virtual byte[] ExportRSAPublicKey()
	{
		throw null;
	}

	public string ExportRSAPublicKeyPem()
	{
		throw null;
	}

	public override void FromXmlString(string xmlString)
	{
	}

	public int GetMaxOutputSize()
	{
		throw null;
	}

	protected virtual byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	protected virtual byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
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

	public override void ImportFromEncryptedPem(ReadOnlySpan<char> input, ReadOnlySpan<byte> passwordBytes)
	{
	}

	public override void ImportFromEncryptedPem(ReadOnlySpan<char> input, ReadOnlySpan<char> password)
	{
	}

	public override void ImportFromPem(ReadOnlySpan<char> input)
	{
	}

	public abstract void ImportParameters(RSAParameters parameters);

	public override void ImportPkcs8PrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public virtual void ImportRSAPrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public virtual void ImportRSAPublicKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public virtual byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public virtual byte[] SignData(Stream data, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public byte[] SignData(ReadOnlySpan<byte> data, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public int SignData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public virtual byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public byte[] SignHash(ReadOnlySpan<byte> hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public int SignHash(ReadOnlySpan<byte> hash, Span<byte> destination, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public override string ToXmlString(bool includePrivateParameters)
	{
		throw null;
	}

	public virtual bool TryDecrypt(ReadOnlySpan<byte> data, Span<byte> destination, RSAEncryptionPadding padding, out int bytesWritten)
	{
		throw null;
	}

	public virtual bool TryEncrypt(ReadOnlySpan<byte> data, Span<byte> destination, RSAEncryptionPadding padding, out int bytesWritten)
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

	public virtual bool TryExportRSAPrivateKey(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryExportRSAPrivateKeyPem(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public virtual bool TryExportRSAPublicKey(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryExportRSAPublicKeyPem(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public override bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	protected virtual bool TryHashData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, out int bytesWritten)
	{
		throw null;
	}

	public virtual bool TrySignData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding, out int bytesWritten)
	{
		throw null;
	}

	public virtual bool TrySignHash(ReadOnlySpan<byte> hash, Span<byte> destination, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding, out int bytesWritten)
	{
		throw null;
	}

	public bool VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public virtual bool VerifyData(byte[] data, int offset, int count, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public bool VerifyData(Stream data, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public virtual bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public virtual bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}

	public virtual bool VerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw null;
	}
}
