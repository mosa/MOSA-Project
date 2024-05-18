namespace System.Net;

public interface ICredentialsByHost
{
	NetworkCredential? GetCredential(string host, int port, string authenticationType);
}
