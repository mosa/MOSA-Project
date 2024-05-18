namespace System.Net;

public interface IWebProxy
{
	ICredentials? Credentials { get; set; }

	Uri? GetProxy(Uri destination);

	bool IsBypassed(Uri host);
}
