using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Sockets;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class SocketTaskExtensions
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task<Socket> AcceptAsync(this Socket socket)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task<Socket> AcceptAsync(this Socket socket, Socket? acceptSocket)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task ConnectAsync(this Socket socket, EndPoint remoteEP)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static ValueTask ConnectAsync(this Socket socket, EndPoint remoteEP, CancellationToken cancellationToken)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task ConnectAsync(this Socket socket, IPAddress address, int port)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static ValueTask ConnectAsync(this Socket socket, IPAddress address, int port, CancellationToken cancellationToken)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task ConnectAsync(this Socket socket, IPAddress[] addresses, int port)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static ValueTask ConnectAsync(this Socket socket, IPAddress[] addresses, int port, CancellationToken cancellationToken)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task ConnectAsync(this Socket socket, string host, int port)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static ValueTask ConnectAsync(this Socket socket, string host, int port, CancellationToken cancellationToken)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task<int> ReceiveAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task<int> ReceiveAsync(this Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static ValueTask<int> ReceiveAsync(this Socket socket, Memory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task<SocketReceiveFromResult> ReceiveFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task<int> SendAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task<int> SendAsync(this Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static ValueTask<int> SendAsync(this Socket socket, ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Task<int> SendToAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEP)
	{
		throw null;
	}
}
