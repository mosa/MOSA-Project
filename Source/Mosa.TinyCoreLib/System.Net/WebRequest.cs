using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Threading.Tasks;

namespace System.Net;

public abstract class WebRequest : MarshalByRefObject, ISerializable
{
	public AuthenticationLevel AuthenticationLevel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual RequestCachePolicy? CachePolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string? ConnectionGroupName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual long ContentLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string? ContentType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual ICredentials? Credentials
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public static RequestCachePolicy? DefaultCachePolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static IWebProxy? DefaultWebProxy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual WebHeaderCollection Headers
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TokenImpersonationLevel ImpersonationLevel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string Method
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool PreAuthenticate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual IWebProxy? Proxy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual Uri RequestUri
	{
		get
		{
			throw null;
		}
	}

	public virtual int Timeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool UseDefaultCredentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected WebRequest()
	{
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected WebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public virtual void Abort()
	{
	}

	public virtual IAsyncResult BeginGetRequestStream(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public virtual IAsyncResult BeginGetResponse(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static WebRequest Create(string requestUriString)
	{
		throw null;
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static WebRequest Create(Uri requestUri)
	{
		throw null;
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static WebRequest CreateDefault(Uri requestUri)
	{
		throw null;
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static HttpWebRequest CreateHttp(string requestUriString)
	{
		throw null;
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static HttpWebRequest CreateHttp(Uri requestUri)
	{
		throw null;
	}

	public virtual Stream EndGetRequestStream(IAsyncResult asyncResult)
	{
		throw null;
	}

	public virtual WebResponse EndGetResponse(IAsyncResult asyncResult)
	{
		throw null;
	}

	[Obsolete("Serialization has been deprecated for WebRequest.")]
	protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public virtual Stream GetRequestStream()
	{
		throw null;
	}

	public virtual Task<Stream> GetRequestStreamAsync()
	{
		throw null;
	}

	public virtual WebResponse GetResponse()
	{
		throw null;
	}

	public virtual Task<WebResponse> GetResponseAsync()
	{
		throw null;
	}

	public static IWebProxy GetSystemWebProxy()
	{
		throw null;
	}

	public static bool RegisterPrefix(string prefix, IWebRequestCreate creator)
	{
		throw null;
	}

	[Obsolete("Serialization has been deprecated for WebRequest.")]
	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
