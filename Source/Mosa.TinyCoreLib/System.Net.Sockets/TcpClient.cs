using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Sockets;

public class TcpClient : IDisposable
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

	public bool Connected
	{
		get
		{
			throw null;
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

	public LingerOption? LingerState
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

	public bool NoDelay
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ReceiveBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ReceiveTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int SendBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int SendTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TcpClient()
	{
	}

	public TcpClient(IPEndPoint localEP)
	{
	}

	public TcpClient(AddressFamily family)
	{
	}

	public TcpClient(string hostname, int port)
	{
	}

	public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginConnect(string host, int port, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public void Close()
	{
	}

	public void Connect(IPAddress address, int port)
	{
	}

	public void Connect(IPAddress[] ipAddresses, int port)
	{
	}

	public void Connect(IPEndPoint remoteEP)
	{
	}

	public void Connect(string hostname, int port)
	{
	}

	public Task ConnectAsync(IPAddress address, int port)
	{
		throw null;
	}

	public ValueTask ConnectAsync(IPAddress address, int port, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ConnectAsync(IPAddress[] addresses, int port)
	{
		throw null;
	}

	public ValueTask ConnectAsync(IPAddress[] addresses, int port, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ConnectAsync(IPEndPoint remoteEP)
	{
		throw null;
	}

	public ValueTask ConnectAsync(IPEndPoint remoteEP, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ConnectAsync(string host, int port)
	{
		throw null;
	}

	public ValueTask ConnectAsync(string host, int port, CancellationToken cancellationToken)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void EndConnect(IAsyncResult asyncResult)
	{
	}

	~TcpClient()
	{
	}

	public NetworkStream GetStream()
	{
		throw null;
	}
}
