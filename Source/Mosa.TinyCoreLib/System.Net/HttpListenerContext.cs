using System.Net.WebSockets;
using System.Security.Principal;
using System.Threading.Tasks;

namespace System.Net;

public sealed class HttpListenerContext
{
	public HttpListenerRequest Request
	{
		get
		{
			throw null;
		}
	}

	public HttpListenerResponse Response
	{
		get
		{
			throw null;
		}
	}

	public IPrincipal? User
	{
		get
		{
			throw null;
		}
	}

	internal HttpListenerContext()
	{
	}

	public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(string? subProtocol)
	{
		throw null;
	}

	public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(string? subProtocol, int receiveBufferSize, TimeSpan keepAliveInterval)
	{
		throw null;
	}

	public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(string? subProtocol, int receiveBufferSize, TimeSpan keepAliveInterval, ArraySegment<byte> internalBuffer)
	{
		throw null;
	}

	public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(string? subProtocol, TimeSpan keepAliveInterval)
	{
		throw null;
	}
}
