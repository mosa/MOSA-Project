using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace System.Net;

public sealed class HttpListenerResponse : IDisposable
{
	public Encoding? ContentEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long ContentLength64
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? ContentType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CookieCollection Cookies
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public WebHeaderCollection Headers
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

	public Stream OutputStream
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
		set
		{
		}
	}

	public string? RedirectLocation
	{
		get
		{
			throw null;
		}
		set
		{
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

	public int StatusCode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string StatusDescription
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal HttpListenerResponse()
	{
	}

	public void Abort()
	{
	}

	public void AddHeader(string name, string value)
	{
	}

	public void AppendCookie(Cookie cookie)
	{
	}

	public void AppendHeader(string name, string value)
	{
	}

	public void Close()
	{
	}

	public void Close(byte[] responseEntity, bool willBlock)
	{
	}

	public void CopyFrom(HttpListenerResponse templateResponse)
	{
	}

	public void Redirect([StringSyntax("Uri")] string url)
	{
	}

	public void SetCookie(Cookie cookie)
	{
	}

	void IDisposable.Dispose()
	{
	}
}
