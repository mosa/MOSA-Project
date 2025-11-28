using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace System.Net.Http;

public class HttpRequestMessage : IDisposable
{
	public HttpContent? Content
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpRequestHeaders Headers
	{
		get
		{
			throw null;
		}
	}

	public HttpMethod Method
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("HttpRequestMessage.Properties has been deprecated. Use Options instead.")]
	public IDictionary<string, object?> Properties
	{
		get
		{
			throw null;
		}
	}

	public HttpRequestOptions Options
	{
		get
		{
			throw null;
		}
	}

	public Uri? RequestUri
	{
		get
		{
			throw null;
		}
		set
		{
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

	public HttpVersionPolicy VersionPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpRequestMessage()
	{
	}

	public HttpRequestMessage(HttpMethod method, [StringSyntax("Uri")] string? requestUri)
	{
	}

	public HttpRequestMessage(HttpMethod method, Uri? requestUri)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
