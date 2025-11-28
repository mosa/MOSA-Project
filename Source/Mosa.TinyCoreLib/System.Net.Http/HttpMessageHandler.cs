using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public abstract class HttpMessageHandler : IDisposable
{
	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	protected internal virtual HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}

	protected internal abstract Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
}
