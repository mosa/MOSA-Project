using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets;

public abstract class WebSocket : IDisposable
{
	public abstract WebSocketCloseStatus? CloseStatus { get; }

	public abstract string? CloseStatusDescription { get; }

	public static TimeSpan DefaultKeepAliveInterval
	{
		get
		{
			throw null;
		}
	}

	public abstract WebSocketState State { get; }

	public abstract string? SubProtocol { get; }

	public abstract void Abort();

	public abstract Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken);

	public abstract Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken);

	public static ArraySegment<byte> CreateClientBuffer(int receiveBufferSize, int sendBufferSize)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static WebSocket CreateClientWebSocket(Stream innerStream, string? subProtocol, int receiveBufferSize, int sendBufferSize, TimeSpan keepAliveInterval, bool useZeroMaskingKey, ArraySegment<byte> internalBuffer)
	{
		throw null;
	}

	public static WebSocket CreateFromStream(Stream stream, bool isServer, string? subProtocol, TimeSpan keepAliveInterval)
	{
		throw null;
	}

	public static WebSocket CreateFromStream(Stream stream, WebSocketCreationOptions options)
	{
		throw null;
	}

	public static ArraySegment<byte> CreateServerBuffer(int receiveBufferSize)
	{
		throw null;
	}

	public abstract void Dispose();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.")]
	public static bool IsApplicationTargeting45()
	{
		throw null;
	}

	protected static bool IsStateTerminal(WebSocketState state)
	{
		throw null;
	}

	public abstract Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);

	public virtual ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.")]
	public static void RegisterPrefixes()
	{
	}

	public abstract Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken);

	public virtual ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		throw null;
	}

	public virtual ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, WebSocketMessageFlags messageFlags, CancellationToken cancellationToken)
	{
		throw null;
	}

	protected static void ThrowOnInvalidState(WebSocketState state, params WebSocketState[] validStates)
	{
	}
}
