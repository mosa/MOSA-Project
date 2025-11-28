using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;

namespace System.Net;

public sealed class FtpWebRequest : WebRequest
{
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

	public long ContentOffset
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

	public override ICredentials? Credentials
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

	public bool EnableSsl
	{
		get
		{
			throw null;
		}
		set
		{
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

	public string? RenameTo
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

	public override Uri RequestUri
	{
		get
		{
			throw null;
		}
	}

	public ServicePoint ServicePoint
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

	public bool UseBinary
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

	public bool UsePassive
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal FtpWebRequest()
	{
	}

	public override void Abort()
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

	public override WebResponse EndGetResponse(IAsyncResult asyncResult)
	{
		throw null;
	}

	public override Stream GetRequestStream()
	{
		throw null;
	}

	public override WebResponse GetResponse()
	{
		throw null;
	}
}
