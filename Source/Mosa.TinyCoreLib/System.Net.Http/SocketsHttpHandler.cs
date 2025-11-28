using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net.Security;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

[UnsupportedOSPlatform("browser")]
public sealed class SocketsHttpHandler : HttpMessageHandler
{
	public int InitialHttp2StreamWindowSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatformGuard("browser")]
	public static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

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

	public TimeSpan ConnectTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CookieContainer CookieContainer
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

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

	public TimeSpan Expect100ContinueTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan KeepAlivePingDelay
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan KeepAlivePingTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpKeepAlivePingPolicy KeepAlivePingPolicy
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

	public TimeSpan PooledConnectionIdleTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan PooledConnectionLifetime
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

	public IDictionary<string, object?> Properties
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

	public HeaderEncodingSelector<HttpRequestMessage>? RequestHeaderEncodingSelector
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan ResponseDrainTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HeaderEncodingSelector<HttpRequestMessage>? ResponseHeaderEncodingSelector
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SslClientAuthenticationOptions SslOptions
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

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

	public Func<SocketsHttpConnectionContext, CancellationToken, ValueTask<Stream>>? ConnectCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Func<SocketsHttpPlaintextStreamFilterContext, CancellationToken, ValueTask<Stream>>? PlaintextStreamFilter
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
	public DistributedContextPropagator? ActivityHeadersPropagator
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

	protected internal override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}

	protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}
}
