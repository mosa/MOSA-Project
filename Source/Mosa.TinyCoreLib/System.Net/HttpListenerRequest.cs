using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace System.Net;

public sealed class HttpListenerRequest
{
	public string[]? AcceptTypes
	{
		get
		{
			throw null;
		}
	}

	public int ClientCertificateError
	{
		get
		{
			throw null;
		}
	}

	public Encoding ContentEncoding
	{
		get
		{
			throw null;
		}
	}

	public long ContentLength64
	{
		get
		{
			throw null;
		}
	}

	public string? ContentType
	{
		get
		{
			throw null;
		}
	}

	public CookieCollection Cookies
	{
		get
		{
			throw null;
		}
	}

	public bool HasEntityBody
	{
		get
		{
			throw null;
		}
	}

	public NameValueCollection Headers
	{
		get
		{
			throw null;
		}
	}

	public string HttpMethod
	{
		get
		{
			throw null;
		}
	}

	public Stream InputStream
	{
		get
		{
			throw null;
		}
	}

	public bool IsAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public bool IsLocal
	{
		get
		{
			throw null;
		}
	}

	public bool IsSecureConnection
	{
		get
		{
			throw null;
		}
	}

	public bool IsWebSocketRequest
	{
		get
		{
			throw null;
		}
	}

	public bool KeepAlive
	{
		get
		{
			throw null;
		}
	}

	public IPEndPoint LocalEndPoint
	{
		get
		{
			throw null;
		}
	}

	public Version ProtocolVersion
	{
		get
		{
			throw null;
		}
	}

	public NameValueCollection QueryString
	{
		get
		{
			throw null;
		}
	}

	public string? RawUrl
	{
		get
		{
			throw null;
		}
	}

	public IPEndPoint RemoteEndPoint
	{
		get
		{
			throw null;
		}
	}

	public Guid RequestTraceIdentifier
	{
		get
		{
			throw null;
		}
	}

	public string? ServiceName
	{
		get
		{
			throw null;
		}
	}

	public TransportContext TransportContext
	{
		get
		{
			throw null;
		}
	}

	public Uri? Url
	{
		get
		{
			throw null;
		}
	}

	public Uri? UrlReferrer
	{
		get
		{
			throw null;
		}
	}

	public string UserAgent
	{
		get
		{
			throw null;
		}
	}

	public string UserHostAddress
	{
		get
		{
			throw null;
		}
	}

	public string UserHostName
	{
		get
		{
			throw null;
		}
	}

	public string[]? UserLanguages
	{
		get
		{
			throw null;
		}
	}

	internal HttpListenerRequest()
	{
	}

	public IAsyncResult BeginGetClientCertificate(AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public X509Certificate2? EndGetClientCertificate(IAsyncResult asyncResult)
	{
		throw null;
	}

	public X509Certificate2? GetClientCertificate()
	{
		throw null;
	}

	public Task<X509Certificate2?> GetClientCertificateAsync()
	{
		throw null;
	}
}
