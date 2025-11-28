using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Quic;

public sealed class QuicListener : IAsyncDisposable
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

	internal QuicListener()
	{
	}

	public ValueTask<QuicConnection> AcceptConnectionAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}

	public static ValueTask<QuicListener> ListenAsync(QuicListenerOptions options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
