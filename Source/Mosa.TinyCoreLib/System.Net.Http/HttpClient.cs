using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class HttpClient : HttpMessageInvoker
{
	public Uri? BaseAddress
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static IWebProxy DefaultProxy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpRequestHeaders DefaultRequestHeaders
	{
		get
		{
			throw null;
		}
	}

	public Version DefaultRequestVersion
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpVersionPolicy DefaultVersionPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long MaxResponseContentBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan Timeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpClient()
		: base(null)
	{
	}

	public HttpClient(HttpMessageHandler handler)
		: base(null)
	{
	}

	public HttpClient(HttpMessageHandler handler, bool disposeHandler)
		: base(null)
	{
	}

	public void CancelPendingRequests()
	{
	}

	public Task<HttpResponseMessage> DeleteAsync([StringSyntax("Uri")] string? requestUri)
	{
		throw null;
	}

	public Task<HttpResponseMessage> DeleteAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> DeleteAsync(Uri? requestUri)
	{
		throw null;
	}

	public Task<HttpResponseMessage> DeleteAsync(Uri? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri)
	{
		throw null;
	}

	public Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, HttpCompletionOption completionOption)
	{
		throw null;
	}

	public Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> GetAsync(Uri? requestUri)
	{
		throw null;
	}

	public Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption)
	{
		throw null;
	}

	public Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> GetAsync(Uri? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<byte[]> GetByteArrayAsync([StringSyntax("Uri")] string? requestUri)
	{
		throw null;
	}

	public Task<byte[]> GetByteArrayAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<byte[]> GetByteArrayAsync(Uri? requestUri)
	{
		throw null;
	}

	public Task<byte[]> GetByteArrayAsync(Uri? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<Stream> GetStreamAsync([StringSyntax("Uri")] string? requestUri)
	{
		throw null;
	}

	public Task<Stream> GetStreamAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<Stream> GetStreamAsync(Uri? requestUri)
	{
		throw null;
	}

	public Task<Stream> GetStreamAsync(Uri? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<string> GetStringAsync([StringSyntax("Uri")] string? requestUri)
	{
		throw null;
	}

	public Task<string> GetStringAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<string> GetStringAsync(Uri? requestUri)
	{
		throw null;
	}

	public Task<string> GetStringAsync(Uri? requestUri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PatchAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PatchAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PostAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PostAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PutAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PutAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content)
	{
		throw null;
	}

	public Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public HttpResponseMessage Send(HttpRequestMessage request)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
	{
		throw null;
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
	{
		throw null;
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}
}
