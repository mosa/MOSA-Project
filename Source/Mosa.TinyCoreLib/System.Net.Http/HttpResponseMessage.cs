using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace System.Net.Http;

public class HttpResponseMessage : IDisposable
{
	public HttpContent Content
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

	public HttpResponseHeaders Headers
	{
		get
		{
			throw null;
		}
	}

	public bool IsSuccessStatusCode
	{
		get
		{
			throw null;
		}
	}

	public string? ReasonPhrase
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpRequestMessage? RequestMessage
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpStatusCode StatusCode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpResponseHeaders TrailingHeaders
	{
		get
		{
			throw null;
		}
	}

	public Version Version
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpResponseMessage()
	{
	}

	public HttpResponseMessage(HttpStatusCode statusCode)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public HttpResponseMessage EnsureSuccessStatusCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
