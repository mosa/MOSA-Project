using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class Rfc3161TimestampRequest
{
	public bool HasExtensions
	{
		get
		{
			throw null;
		}
	}

	public Oid HashAlgorithmId
	{
		get
		{
			throw null;
		}
	}

	public Oid? RequestedPolicyId
	{
		get
		{
			throw null;
		}
	}

	public bool RequestSignerCertificate
	{
		get
		{
			throw null;
		}
	}

	public int Version
	{
		get
		{
			throw null;
		}
	}

	internal Rfc3161TimestampRequest()
	{
	}

	public static Rfc3161TimestampRequest CreateFromData(ReadOnlySpan<byte> data, HashAlgorithmName hashAlgorithm, Oid? requestedPolicyId = null, ReadOnlyMemory<byte>? nonce = null, bool requestSignerCertificates = false, X509ExtensionCollection? extensions = null)
	{
		throw null;
	}

	public static Rfc3161TimestampRequest CreateFromHash(ReadOnlyMemory<byte> hash, HashAlgorithmName hashAlgorithm, Oid? requestedPolicyId = null, ReadOnlyMemory<byte>? nonce = null, bool requestSignerCertificates = false, X509ExtensionCollection? extensions = null)
	{
		throw null;
	}

	public static Rfc3161TimestampRequest CreateFromHash(ReadOnlyMemory<byte> hash, Oid hashAlgorithmId, Oid? requestedPolicyId = null, ReadOnlyMemory<byte>? nonce = null, bool requestSignerCertificates = false, X509ExtensionCollection? extensions = null)
	{
		throw null;
	}

	public static Rfc3161TimestampRequest CreateFromSignerInfo(SignerInfo signerInfo, HashAlgorithmName hashAlgorithm, Oid? requestedPolicyId = null, ReadOnlyMemory<byte>? nonce = null, bool requestSignerCertificates = false, X509ExtensionCollection? extensions = null)
	{
		throw null;
	}

	public byte[] Encode()
	{
		throw null;
	}

	public X509ExtensionCollection GetExtensions()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> GetMessageHash()
	{
		throw null;
	}

	public ReadOnlyMemory<byte>? GetNonce()
	{
		throw null;
	}

	public Rfc3161TimestampToken ProcessResponse(ReadOnlyMemory<byte> responseBytes, out int bytesConsumed)
	{
		throw null;
	}

	public static bool TryDecode(ReadOnlyMemory<byte> encodedBytes, [NotNullWhen(true)] out Rfc3161TimestampRequest? request, out int bytesConsumed)
	{
		throw null;
	}

	public bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
