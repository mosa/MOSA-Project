using System.ComponentModel;
using System.IO;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace System.Net;

public class HttpWebRequest : WebRequest, ISerializable
{
	public string? Accept
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Uri Address
	{
		get
		{
			throw null;
		}
	}

	public virtual bool AllowAutoRedirect
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool AllowReadStreamBuffering
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool AllowWriteStreamBuffering
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

	public string? Connection
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string? ConnectionGroupName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override long ContentLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string? ContentType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpContinueDelegate? ContinueDelegate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ContinueTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual CookieContainer? CookieContainer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override ICredentials? Credentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime Date
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public new static RequestCachePolicy? DefaultCachePolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int DefaultMaximumErrorResponseLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int DefaultMaximumResponseHeadersLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Expect
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool HaveResponse
	{
		get
		{
			throw null;
		}
	}

	public override WebHeaderCollection Headers
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Host
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime IfModifiedSince
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool KeepAlive
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MaximumAutomaticRedirections
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MaximumResponseHeadersLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? MediaType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string Method
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Pipelined
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override bool PreAuthenticate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Version ProtocolVersion
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override IWebProxy? Proxy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ReadWriteTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Referer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override Uri RequestUri
	{
		get
		{
			throw null;
		}
	}

	public bool SendChunked
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RemoteCertificateValidationCallback? ServerCertificateValidationCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ServicePoint ServicePoint
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsCookieContainer
	{
		get
		{
			throw null;
		}
	}

	public override int Timeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? TransferEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool UnsafeAuthenticatedConnectionSharing
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override bool UseDefaultCredentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? UserAgent
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected HttpWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public override void Abort()
	{
	}

	public void AddRange(int range)
	{
	}

	public void AddRange(int from, int to)
	{
	}

	public void AddRange(long range)
	{
	}

	public void AddRange(long from, long to)
	{
	}

	public void AddRange(string rangeSpecifier, int range)
	{
	}

	public void AddRange(string rangeSpecifier, int from, int to)
	{
	}

	public void AddRange(string rangeSpecifier, long range)
	{
	}

	public void AddRange(string rangeSpecifier, long from, long to)
	{
	}

	public override IAsyncResult BeginGetRequestStream(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public override IAsyncResult BeginGetResponse(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public override Stream EndGetRequestStream(IAsyncResult asyncResult)
	{
		throw null;
	}

	public Stream EndGetRequestStream(IAsyncResult asyncResult, out TransportContext? context)
	{
		throw null;
	}

	public override WebResponse EndGetResponse(IAsyncResult asyncResult)
	{
		throw null;
	}

	[Obsolete("Serialization has been deprecated for HttpWebRequest.")]
	protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public override Stream GetRequestStream()
	{
		throw null;
	}

	public Stream GetRequestStream(out TransportContext? context)
	{
		throw null;
	}

	public override WebResponse GetResponse()
	{
		throw null;
	}

	[Obsolete("Serialization has been deprecated for HttpWebRequest.")]
	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
