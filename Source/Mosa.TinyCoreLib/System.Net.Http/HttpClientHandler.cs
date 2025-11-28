using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Security;
using System.Runtime.Versioning;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class HttpClientHandler : HttpMessageHandler
{
	public bool AllowAutoRedirect
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public DecompressionMethods AutomaticDecompression
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public bool CheckCertificateRevocationList
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ClientCertificateOption ClientCertificateOptions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public X509CertificateCollection ClientCertificates
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("browser")]
	public CookieContainer CookieContainer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public ICredentials? Credentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool> DangerousAcceptAnyServerCertificateValidator
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("browser")]
	public ICredentials? DefaultProxyCredentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public int MaxAutomaticRedirections
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public int MaxConnectionsPerServer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long MaxRequestContentBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public int MaxResponseHeadersLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[CLSCompliant(false)]
	public IMeterFactory? MeterFactory
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public bool PreAuthenticate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IDictionary<string, object?> Properties
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public IWebProxy? Proxy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? ServerCertificateCustomValidationCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public SslProtocols SslProtocols
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool SupportsAutomaticDecompression
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsProxy
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsRedirectConfiguration
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("browser")]
	public bool UseCookies
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public bool UseDefaultCredentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public bool UseProxy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected override void Dispose(bool disposing)
	{
	}

	[UnsupportedOSPlatform("browser")]
	protected internal override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}

	protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}
}
