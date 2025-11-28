using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public abstract class DSA : AsymmetricAlgorithm
{
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public new static DSA Create()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static DSA Create(int keySizeInBits)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static DSA Create(DSAParameters parameters)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static DSA? Create(string algName)
	{
		throw null;
	}

	public abstract byte[] CreateSignature(byte[] rgbHash);

	public byte[] CreateSignature(byte[] rgbHash, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	protected virtual byte[] CreateSignatureCore(ReadOnlySpan<byte> hash, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public abstract DSAParameters ExportParameters(bool includePrivateParameters);

	public override void FromXmlString(string xmlString)
	{
	}

	public int GetMaxSignatureSize(DSASignatureFormat signatureFormat)
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

	public abstract void ImportParameters(DSAParameters parameters);

	public override void ImportPkcs8PrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public virtual byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public virtual byte[] SignData(Stream data, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public byte[] SignData(Stream data, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	protected virtual byte[] SignDataCore(Stream data, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	protected virtual byte[] SignDataCore(ReadOnlySpan<byte> data, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public override string ToXmlString(bool includePrivateParameters)
	{
		throw null;
	}

	public virtual bool TryCreateSignature(ReadOnlySpan<byte> hash, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryCreateSignature(ReadOnlySpan<byte> hash, Span<byte> destination, DSASignatureFormat signatureFormat, out int bytesWritten)
	{
		throw null;
	}

	protected virtual bool TryCreateSignatureCore(ReadOnlySpan<byte> hash, Span<byte> destination, DSASignatureFormat signatureFormat, out int bytesWritten)
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

	public override bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	protected virtual bool TryHashData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, out int bytesWritten)
	{
		throw null;
	}

	public virtual bool TrySignData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, out int bytesWritten)
	{
		throw null;
	}

	public bool TrySignData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat, out int bytesWritten)
	{
		throw null;
	}

	protected virtual bool TrySignDataCore(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat, out int bytesWritten)
	{
		throw null;
	}

	public bool VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public bool VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public virtual bool VerifyData(byte[] data, int offset, int count, byte[] signature, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public bool VerifyData(byte[] data, int offset, int count, byte[] signature, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public virtual bool VerifyData(Stream data, byte[] signature, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public bool VerifyData(Stream data, byte[] signature, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public virtual bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	protected virtual bool VerifyDataCore(Stream data, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	protected virtual bool VerifyDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public abstract bool VerifySignature(byte[] rgbHash, byte[] rgbSignature);

	public bool VerifySignature(byte[] rgbHash, byte[] rgbSignature, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public virtual bool VerifySignature(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature)
	{
		throw null;
	}

	public bool VerifySignature(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	protected virtual bool VerifySignatureCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, DSASignatureFormat signatureFormat)
	{
		throw null;
	}
}
