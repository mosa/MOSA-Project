using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class HttpMessageInvoker : IDisposable
{
	public HttpMessageInvoker(HttpMessageHandler handler)
	{
	}

	public HttpMessageInvoker(HttpMessageHandler handler, bool disposeHandler)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public virtual HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}

	public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}
}
