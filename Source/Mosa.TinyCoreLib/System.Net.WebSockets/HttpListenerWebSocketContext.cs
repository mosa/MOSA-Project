using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;

namespace System.Net.WebSockets;

public class HttpListenerWebSocketContext : WebSocketContext
{
	public override CookieCollection CookieCollection
	{
		get
		{
			throw null;
		}
	}

	public override NameValueCollection Headers
	{
		get
		{
			throw null;
		}
	}

	public override bool IsAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public override bool IsLocal
	{
		get
		{
			throw null;
		}
	}

	public override bool IsSecureConnection
	{
		get
		{
			throw null;
		}
	}

	public override string Origin
	{
		get
		{
			throw null;
		}
	}

	public override Uri RequestUri
	{
		get
		{
			throw null;
		}
	}

	public override string SecWebSocketKey
	{
		get
		{
			throw null;
		}
	}

	public override IEnumerable<string> SecWebSocketProtocols
	{
		get
		{
			throw null;
		}
	}

	public override string SecWebSocketVersion
	{
		get
		{
			throw null;
		}
	}

	public override IPrincipal User
	{
		get
		{
			throw null;
		}
	}

	public override WebSocket WebSocket
	{
		get
		{
			throw null;
		}
	}

	internal HttpListenerWebSocketContext()
	{
	}
}
