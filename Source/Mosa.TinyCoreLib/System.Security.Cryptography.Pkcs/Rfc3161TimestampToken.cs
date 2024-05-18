using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class Rfc3161TimestampToken
{
	public Rfc3161TimestampTokenInfo TokenInfo
	{
		get
		{
			throw null;
		}
	}

	internal Rfc3161TimestampToken()
	{
	}

	public SignedCms AsSignedCms()
	{
		throw null;
	}

	public static bool TryDecode(ReadOnlyMemory<byte> encodedBytes, [NotNullWhen(true)] out Rfc3161TimestampToken? token, out int bytesConsumed)
	{
		throw null;
	}

	public bool VerifySignatureForData(ReadOnlySpan<byte> data, [NotNullWhen(true)] out X509Certificate2? signerCertificate, X509Certificate2Collection? extraCandidates = null)
	{
		throw null;
	}

	public bool VerifySignatureForHash(ReadOnlySpan<byte> hash, HashAlgorithmName hashAlgorithm, [NotNullWhen(true)] out X509Certificate2? signerCertificate, X509Certificate2Collection? extraCandidates = null)
	{
		throw null;
	}

	public bool VerifySignatureForHash(ReadOnlySpan<byte> hash, Oid hashAlgorithmId, [NotNullWhen(true)] out X509Certificate2? signerCertificate, X509Certificate2Collection? extraCandidates = null)
	{
		throw null;
	}

	public bool VerifySignatureForSignerInfo(SignerInfo signerInfo, [NotNullWhen(true)] out X509Certificate2? signerCertificate, X509Certificate2Collection? extraCandidates = null)
	{
		throw null;
	}
}
