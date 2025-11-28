using System.Security.Authentication;

namespace System.Net.Security;

public readonly struct SslClientHelloInfo
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public string ServerName
	{
		get
		{
			throw null;
		}
	}

	public SslProtocols SslProtocols
	{
		get
		{
			throw null;
		}
	}

	public SslClientHelloInfo(string serverName, SslProtocols sslProtocols)
	{
		throw null;
	}
}
