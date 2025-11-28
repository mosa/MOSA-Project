using System.Security.Principal;

namespace System.Net;

public class HttpListenerBasicIdentity : GenericIdentity
{
	public virtual string Password
	{
		get
		{
			throw null;
		}
	}

	public HttpListenerBasicIdentity(string username, string password)
		: base((GenericIdentity)null)
	{
	}
}
