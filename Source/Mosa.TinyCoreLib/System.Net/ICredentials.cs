namespace System.Net;

public interface ICredentials
{
	NetworkCredential? GetCredential(Uri uri, string authType);
}
