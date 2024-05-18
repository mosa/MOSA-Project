using System.Net.Security;

namespace System.Net.Quic;

public sealed class QuicClientConnectionOptions : QuicConnectionOptions
{
	public SslClientAuthenticationOptions ClientAuthenticationOptions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IPEndPoint? LocalEndPoint
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EndPoint RemoteEndPoint
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
