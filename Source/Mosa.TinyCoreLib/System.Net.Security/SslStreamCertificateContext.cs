using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security;

public class SslStreamCertificateContext
{
	public ReadOnlyCollection<X509Certificate2> IntermediateCertificates
	{
		get
		{
			throw null;
		}
	}

	public X509Certificate2 TargetCertificate
	{
		get
		{
			throw null;
		}
	}

	internal SslStreamCertificateContext()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static SslStreamCertificateContext Create(X509Certificate2 target, X509Certificate2Collection? additionalCertificates, bool offline)
	{
		throw null;
	}

	public static SslStreamCertificateContext Create(X509Certificate2 target, X509Certificate2Collection? additionalCertificates, bool offline = false, SslCertificateTrust? trust = null)
	{
		throw null;
	}
}
