using System.Collections.Generic;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class WinHttpHandler : HttpMessageHandler
{
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

	public bool AutomaticRedirection
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public ClientCertificateOption ClientCertificateOption
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509Certificate2Collection ClientCertificates
	{
		get
		{
			throw null;
		}
	}

	public CookieContainer? CookieContainer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CookieUsePolicy CookieUsePolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public bool EnableMultipleHttp2Connections
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public int MaxResponseDrainSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public IDictionary<string, object> Properties
	{
		get
		{
			throw null;
		}
	}

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

	public TimeSpan ReceiveDataTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan ReceiveHeadersTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan SendTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>? ServerCertificateValidationCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ICredentials? ServerCredentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public bool TcpKeepAliveEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan TcpKeepAliveTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan TcpKeepAliveInterval
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public WindowsProxyUsePolicy WindowsProxyUsePolicy
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

	protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}
}
