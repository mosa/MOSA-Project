using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security;

public sealed class SslCertificateTrust
{
	internal SslCertificateTrust()
	{
	}

	public static SslCertificateTrust CreateForX509Collection(X509Certificate2Collection trustList, bool sendTrustInHandshake = false)
	{
		throw null;
	}

	public static SslCertificateTrust CreateForX509Store(X509Store store, bool sendTrustInHandshake = false)
	{
		throw null;
	}
}
