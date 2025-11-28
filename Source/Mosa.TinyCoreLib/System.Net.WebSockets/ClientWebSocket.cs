using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets;

public sealed class ClientWebSocket : WebSocket
{
	public override WebSocketCloseStatus? CloseStatus
	{
		get
		{
			throw null;
		}
	}

	public override string? CloseStatusDescription
	{
		get
		{
			throw null;
		}
	}

	public HttpStatusCode HttpStatusCode
	{
		get
		{
			throw null;
		}
	}

	public IReadOnlyDictionary<string, IEnumerable<string>>? HttpResponseHeaders
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ClientWebSocketOptions Options
	{
		get
		{
			throw null;
		}
	}

	public override WebSocketState State
	{
		get
		{
			throw null;
		}
	}

	public override string? SubProtocol
	{
		get
		{
			throw null;
		}
	}

	public override void Abort()
	{
	}

	public override Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ConnectAsync(Uri uri, HttpMessageInvoker? invoker, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override void Dispose()
	{
	}

	public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, WebSocketMessageFlags messageFlags, CancellationToken cancellationToken)
	{
		throw null;
	}
}
