using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;

namespace System.Net.WebSockets;

public abstract class WebSocketContext
{
	public abstract CookieCollection CookieCollection { get; }

	public abstract NameValueCollection Headers { get; }

	public abstract bool IsAuthenticated { get; }

	public abstract bool IsLocal { get; }

	public abstract bool IsSecureConnection { get; }

	public abstract string Origin { get; }

	public abstract Uri RequestUri { get; }

	public abstract string SecWebSocketKey { get; }

	public abstract IEnumerable<string> SecWebSocketProtocols { get; }

	public abstract string SecWebSocketVersion { get; }

	public abstract IPrincipal? User { get; }

	public abstract WebSocket WebSocket { get; }
}
