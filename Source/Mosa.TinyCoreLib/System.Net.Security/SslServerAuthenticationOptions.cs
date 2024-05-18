using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security;

public class SslServerAuthenticationOptions
{
	public bool AllowRenegotiation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool AllowTlsResume
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public List<SslApplicationProtocol>? ApplicationProtocols
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509ChainPolicy? CertificateChainPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509RevocationMode CertificateRevocationCheckMode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CipherSuitesPolicy? CipherSuitesPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ClientCertificateRequired
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SslProtocols EnabledSslProtocols
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EncryptionPolicy EncryptionPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RemoteCertificateValidationCallback? RemoteCertificateValidationCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509Certificate? ServerCertificate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SslStreamCertificateContext? ServerCertificateContext
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ServerCertificateSelectionCallback? ServerCertificateSelectionCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
