using System.Runtime.Versioning;

namespace System.Security.Cryptography.X509Certificates;

[UnsupportedOSPlatform("ios")]
[UnsupportedOSPlatform("tvos")]
public static class DSACertificateExtensions
{
	public static X509Certificate2 CopyWithPrivateKey(this X509Certificate2 certificate, DSA privateKey)
	{
		throw null;
	}

	public static DSA? GetDSAPrivateKey(this X509Certificate2 certificate)
	{
		throw null;
	}

	public static DSA? GetDSAPublicKey(this X509Certificate2 certificate)
	{
		throw null;
	}
}
