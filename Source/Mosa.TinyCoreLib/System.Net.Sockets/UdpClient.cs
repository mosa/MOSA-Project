using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Sockets;

public class UdpClient : IDisposable
{
	protected bool Active
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Available
	{
		get
		{
			throw null;
		}
	}

	public Socket Client
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool DontFragment
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool EnableBroadcast
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ExclusiveAddressUse
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool MulticastLoopback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public short Ttl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public UdpClient()
	{
	}

	public UdpClient(int port)
	{
	}

	public UdpClient(int port, AddressFamily family)
	{
	}

	public UdpClient(IPEndPoint localEP)
	{
	}

	public UdpClient(AddressFamily family)
	{
	}

	public UdpClient(string hostname, int port)
	{
	}

	[SupportedOSPlatform("windows")]
	public void AllowNatTraversal(bool allowed)
	{
	}

	public IAsyncResult BeginReceive(AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginSend(byte[] datagram, int bytes, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginSend(byte[] datagram, int bytes, IPEndPoint? endPoint, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginSend(byte[] datagram, int bytes, string? hostname, int port, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public void Close()
	{
	}

	public void Connect(IPAddress addr, int port)
	{
	}

	public void Connect(IPEndPoint endPoint)
	{
	}

	public void Connect(string hostname, int port)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void DropMulticastGroup(IPAddress multicastAddr)
	{
	}

	public void DropMulticastGroup(IPAddress multicastAddr, int ifindex)
	{
	}

	public byte[] EndReceive(IAsyncResult asyncResult, ref IPEndPoint? remoteEP)
	{
		throw null;
	}

	public int EndSend(IAsyncResult asyncResult)
	{
		throw null;
	}

	public void JoinMulticastGroup(int ifindex, IPAddress multicastAddr)
	{
	}

	public void JoinMulticastGroup(IPAddress multicastAddr)
	{
	}

	public void JoinMulticastGroup(IPAddress multicastAddr, int timeToLive)
	{
	}

	public void JoinMulticastGroup(IPAddress multicastAddr, IPAddress localAddress)
	{
	}

	public byte[] Receive([NotNull] ref IPEndPoint? remoteEP)
	{
		throw null;
	}

	public Task<UdpReceiveResult> ReceiveAsync()
	{
		throw null;
	}

	public ValueTask<UdpReceiveResult> ReceiveAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public int Send(byte[] dgram, int bytes)
	{
		throw null;
	}

	public int Send(byte[] dgram, int bytes, IPEndPoint? endPoint)
	{
		throw null;
	}

	public int Send(byte[] dgram, int bytes, string? hostname, int port)
	{
		throw null;
	}

	public int Send(ReadOnlySpan<byte> datagram)
	{
		throw null;
	}

	public int Send(ReadOnlySpan<byte> datagram, IPEndPoint? endPoint)
	{
		throw null;
	}

	public int Send(ReadOnlySpan<byte> datagram, string? hostname, int port)
	{
		throw null;
	}

	public Task<int> SendAsync(byte[] datagram, int bytes)
	{
		throw null;
	}

	public Task<int> SendAsync(byte[] datagram, int bytes, IPEndPoint? endPoint)
	{
		throw null;
	}

	public Task<int> SendAsync(byte[] datagram, int bytes, string? hostname, int port)
	{
		throw null;
	}

	public ValueTask<int> SendAsync(ReadOnlyMemory<byte> datagram, IPEndPoint? endPoint, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<int> SendAsync(ReadOnlyMemory<byte> datagram, string? hostname, int port, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<int> SendAsync(ReadOnlyMemory<byte> datagram, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
