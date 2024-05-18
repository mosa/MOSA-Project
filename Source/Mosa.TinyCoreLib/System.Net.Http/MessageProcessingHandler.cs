using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public abstract class MessageProcessingHandler : DelegatingHandler
{
	protected MessageProcessingHandler()
	{
	}

	protected MessageProcessingHandler(HttpMessageHandler innerHandler)
	{
	}

	protected abstract HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken);

	protected abstract HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken);

	protected internal sealed override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}

	protected internal sealed override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}
}
