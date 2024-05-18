using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public abstract class DelegatingHandler : HttpMessageHandler
{
	public HttpMessageHandler? InnerHandler
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

	protected DelegatingHandler()
	{
	}

	protected DelegatingHandler(HttpMessageHandler innerHandler)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected internal override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}

	protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw null;
	}
}
