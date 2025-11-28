using System.Collections.ObjectModel;
using System.Runtime.Versioning;

namespace System.Security.Cryptography.X509Certificates;

[UnsupportedOSPlatform("browser")]
public sealed class CertificateRequest
{
	public Collection<X509Extension> CertificateExtensions
	{
		get
		{
			throw null;
		}
	}

	public HashAlgorithmName HashAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public Collection<AsnEncodedData> OtherRequestAttributes
	{
		get
		{
			throw null;
		}
	}

	public PublicKey PublicKey
	{
		get
		{
			throw null;
		}
	}

	public X500DistinguishedName SubjectName
	{
		get
		{
			throw null;
		}
	}

	public CertificateRequest(X500DistinguishedName subjectName, ECDsa key, HashAlgorithmName hashAlgorithm)
	{
	}

	public CertificateRequest(X500DistinguishedName subjectName, RSA key, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
	}

	public CertificateRequest(X500DistinguishedName subjectName, PublicKey publicKey, HashAlgorithmName hashAlgorithm)
	{
	}

	public CertificateRequest(X500DistinguishedName subjectName, PublicKey publicKey, HashAlgorithmName hashAlgorithm, RSASignaturePadding? rsaSignaturePadding = null)
	{
	}

	public CertificateRequest(string subjectName, ECDsa key, HashAlgorithmName hashAlgorithm)
	{
	}

	public CertificateRequest(string subjectName, RSA key, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
	}

	public X509Certificate2 Create(X500DistinguishedName issuerName, X509SignatureGenerator generator, DateTimeOffset notBefore, DateTimeOffset notAfter, byte[] serialNumber)
	{
		throw null;
	}

	public X509Certificate2 Create(X500DistinguishedName issuerName, X509SignatureGenerator generator, DateTimeOffset notBefore, DateTimeOffset notAfter, ReadOnlySpan<byte> serialNumber)
	{
		throw null;
	}

	public X509Certificate2 Create(X509Certificate2 issuerCertificate, DateTimeOffset notBefore, DateTimeOffset notAfter, byte[] serialNumber)
	{
		throw null;
	}

	public X509Certificate2 Create(X509Certificate2 issuerCertificate, DateTimeOffset notBefore, DateTimeOffset notAfter, ReadOnlySpan<byte> serialNumber)
	{
		throw null;
	}

	public X509Certificate2 CreateSelfSigned(DateTimeOffset notBefore, DateTimeOffset notAfter)
	{
		throw null;
	}

	public byte[] CreateSigningRequest()
	{
		throw null;
	}

	public byte[] CreateSigningRequest(X509SignatureGenerator signatureGenerator)
	{
		throw null;
	}

	public string CreateSigningRequestPem()
	{
		throw null;
	}

	public string CreateSigningRequestPem(X509SignatureGenerator signatureGenerator)
	{
		throw null;
	}

	public static CertificateRequest LoadSigningRequest(byte[] pkcs10, HashAlgorithmName signerHashAlgorithm, CertificateRequestLoadOptions options = CertificateRequestLoadOptions.Default, RSASignaturePadding? signerSignaturePadding = null)
	{
		throw null;
	}

	public static CertificateRequest LoadSigningRequest(ReadOnlySpan<byte> pkcs10, HashAlgorithmName signerHashAlgorithm, out int bytesConsumed, CertificateRequestLoadOptions options = CertificateRequestLoadOptions.Default, RSASignaturePadding? signerSignaturePadding = null)
	{
		throw null;
	}

	public static CertificateRequest LoadSigningRequestPem(ReadOnlySpan<char> pkcs10Pem, HashAlgorithmName signerHashAlgorithm, CertificateRequestLoadOptions options = CertificateRequestLoadOptions.Default, RSASignaturePadding? signerSignaturePadding = null)
	{
		throw null;
	}

	public static CertificateRequest LoadSigningRequestPem(string pkcs10Pem, HashAlgorithmName signerHashAlgorithm, CertificateRequestLoadOptions options = CertificateRequestLoadOptions.Default, RSASignaturePadding? signerSignaturePadding = null)
	{
		throw null;
	}
}
