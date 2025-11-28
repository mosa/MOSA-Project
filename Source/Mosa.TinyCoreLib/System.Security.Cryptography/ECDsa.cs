using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public abstract class ECDsa : ECAlgorithm
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
	public new static ECDsa Create()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static ECDsa Create(ECCurve curve)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static ECDsa Create(ECParameters parameters)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static ECDsa? Create(string algorithm)
	{
		throw null;
	}

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

	public virtual byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public virtual byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm)
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

	public byte[] SignData(ReadOnlySpan<byte> data, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public byte[] SignData(ReadOnlySpan<byte> data, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public int SignData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm)
	{
		throw null;
	}

	public int SignData(ReadOnlySpan<byte> data, Span<byte> destination, HashAlgorithmName hashAlgorithm, DSASignatureFormat signatureFormat)
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

	public abstract byte[] SignHash(byte[] hash);

	public byte[] SignHash(byte[] hash, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public byte[] SignHash(ReadOnlySpan<byte> hash)
	{
		throw null;
	}

	public byte[] SignHash(ReadOnlySpan<byte> hash, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public int SignHash(ReadOnlySpan<byte> hash, Span<byte> destination)
	{
		throw null;
	}

	public int SignHash(ReadOnlySpan<byte> hash, Span<byte> destination, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	protected virtual byte[] SignHashCore(ReadOnlySpan<byte> hash, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public override string ToXmlString(bool includePrivateParameters)
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

	public virtual bool TrySignHash(ReadOnlySpan<byte> hash, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TrySignHash(ReadOnlySpan<byte> hash, Span<byte> destination, DSASignatureFormat signatureFormat, out int bytesWritten)
	{
		throw null;
	}

	protected virtual bool TrySignHashCore(ReadOnlySpan<byte> hash, Span<byte> destination, DSASignatureFormat signatureFormat, out int bytesWritten)
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

	public bool VerifyData(Stream data, byte[] signature, HashAlgorithmName hashAlgorithm)
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

	public abstract bool VerifyHash(byte[] hash, byte[] signature);

	public bool VerifyHash(byte[] hash, byte[] signature, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	public virtual bool VerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature)
	{
		throw null;
	}

	public bool VerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, DSASignatureFormat signatureFormat)
	{
		throw null;
	}

	protected virtual bool VerifyHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, DSASignatureFormat signatureFormat)
	{
		throw null;
	}
}
