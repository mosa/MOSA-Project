using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Quic;

public sealed class QuicConnection : IAsyncDisposable
{
	public static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	public IPEndPoint LocalEndPoint
	{
		get
		{
			throw null;
		}
	}

	public SslApplicationProtocol NegotiatedApplicationProtocol
	{
		get
		{
			throw null;
		}
	}

	public X509Certificate? RemoteCertificate
	{
		get
		{
			throw null;
		}
	}

	public IPEndPoint RemoteEndPoint
	{
		get
		{
			throw null;
		}
	}

	public string TargetHostName
	{
		get
		{
			throw null;
		}
	}

	internal QuicConnection()
	{
	}

	public ValueTask<QuicStream> AcceptInboundStreamAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask CloseAsync(long errorCode, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ValueTask<QuicConnection> ConnectAsync(QuicClientConnectionOptions options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}

	public ValueTask<QuicStream> OpenOutboundStreamAsync(QuicStreamType type, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
