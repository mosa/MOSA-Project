using System.Collections.Generic;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Quic;

public sealed class QuicListenerOptions
{
	public List<SslApplicationProtocol> ApplicationProtocols
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Func<QuicConnection, SslClientHelloInfo, CancellationToken, ValueTask<QuicServerConnectionOptions>> ConnectionOptionsCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ListenBacklog
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IPEndPoint ListenEndPoint
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
