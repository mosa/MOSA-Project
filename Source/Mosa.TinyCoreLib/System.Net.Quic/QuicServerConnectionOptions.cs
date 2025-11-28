using System.Net.Security;

namespace System.Net.Quic;

public sealed class QuicServerConnectionOptions : QuicConnectionOptions
{
	public SslServerAuthenticationOptions ServerAuthenticationOptions
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
