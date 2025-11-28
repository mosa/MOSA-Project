using System.Net.Http;
using System.Net.Security;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.WebSockets;

public sealed class ClientWebSocketOptions
{
	[UnsupportedOSPlatform("browser")]
	public X509CertificateCollection ClientCertificates
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
	public CookieContainer? Cookies
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
	public bool CollectHttpResponseDetails
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
	public TimeSpan KeepAliveInterval
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
	public WebSocketDeflateOptions? DangerousDeflateOptions
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

	public Version HttpVersion
	{
		get
		{
			throw null;
		}
		[UnsupportedOSPlatform("browser")]
		set
		{
		}
	}

	public HttpVersionPolicy HttpVersionPolicy
	{
		get
		{
			throw null;
		}
		[UnsupportedOSPlatform("browser")]
		set
		{
		}
	}

	internal ClientWebSocketOptions()
	{
	}

	public void AddSubProtocol(string subProtocol)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public void SetBuffer(int receiveBufferSize, int sendBufferSize)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public void SetBuffer(int receiveBufferSize, int sendBufferSize, ArraySegment<byte> buffer)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public void SetRequestHeader(string headerName, string? headerValue)
	{
	}
}
