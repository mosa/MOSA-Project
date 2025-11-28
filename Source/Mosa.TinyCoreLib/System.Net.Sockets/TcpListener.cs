using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Sockets;

public class TcpListener : IDisposable
{
	protected bool Active
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

	public EndPoint LocalEndpoint
	{
		get
		{
			throw null;
		}
	}

	public Socket Server
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("This constructor has been deprecated. Use TcpListener(IPAddress localaddr, int port) instead.")]
	public TcpListener(int port)
	{
	}

	public TcpListener(IPAddress localaddr, int port)
	{
	}

	public TcpListener(IPEndPoint localEP)
	{
	}

	public Socket AcceptSocket()
	{
		throw null;
	}

	public Task<Socket> AcceptSocketAsync()
	{
		throw null;
	}

	public ValueTask<Socket> AcceptSocketAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public TcpClient AcceptTcpClient()
	{
		throw null;
	}

	public Task<TcpClient> AcceptTcpClientAsync()
	{
		throw null;
	}

	public ValueTask<TcpClient> AcceptTcpClientAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public void AllowNatTraversal(bool allowed)
	{
	}

	public IAsyncResult BeginAcceptSocket(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginAcceptTcpClient(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public static TcpListener Create(int port)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public Socket EndAcceptSocket(IAsyncResult asyncResult)
	{
		throw null;
	}

	public TcpClient EndAcceptTcpClient(IAsyncResult asyncResult)
	{
		throw null;
	}

	public bool Pending()
	{
		throw null;
	}

	public void Start()
	{
	}

	public void Start(int backlog)
	{
	}

	public void Stop()
	{
	}
}
