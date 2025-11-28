using System.Collections.Generic;
using System.Numerics;

namespace System.Security.Cryptography.X509Certificates;

public sealed class CertificateRevocationListBuilder
{
	public void AddEntry(byte[] serialNumber, DateTimeOffset? revocationTime = null, X509RevocationReason? reason = null)
	{
	}

	public void AddEntry(ReadOnlySpan<byte> serialNumber, DateTimeOffset? revocationTime = null, X509RevocationReason? reason = null)
	{
	}

	public void AddEntry(X509Certificate2 certificate, DateTimeOffset? revocationTime = null, X509RevocationReason? reason = null)
	{
	}

	public byte[] Build(X500DistinguishedName issuerName, X509SignatureGenerator generator, BigInteger crlNumber, DateTimeOffset nextUpdate, HashAlgorithmName hashAlgorithm, X509AuthorityKeyIdentifierExtension authorityKeyIdentifier, DateTimeOffset? thisUpdate = null)
	{
		throw null;
	}

	public byte[] Build(X509Certificate2 issuerCertificate, BigInteger crlNumber, DateTimeOffset nextUpdate, HashAlgorithmName hashAlgorithm, RSASignaturePadding? rsaSignaturePadding = null, DateTimeOffset? thisUpdate = null)
	{
		throw null;
	}

	public static X509Extension BuildCrlDistributionPointExtension(IEnumerable<string> uris, bool critical = false)
	{
		throw null;
	}

	public static CertificateRevocationListBuilder Load(byte[] currentCrl, out BigInteger currentCrlNumber)
	{
		throw null;
	}

	public static CertificateRevocationListBuilder Load(ReadOnlySpan<byte> currentCrl, out BigInteger currentCrlNumber, out int bytesConsumed)
	{
		throw null;
	}

	public static CertificateRevocationListBuilder LoadPem(ReadOnlySpan<char> currentCrl, out BigInteger currentCrlNumber)
	{
		throw null;
	}

	public static CertificateRevocationListBuilder LoadPem(string currentCrl, out BigInteger currentCrlNumber)
	{
		throw null;
	}

	public bool RemoveEntry(byte[] serialNumber)
	{
		throw null;
	}

	public bool RemoveEntry(ReadOnlySpan<byte> serialNumber)
	{
		throw null;
	}
}
