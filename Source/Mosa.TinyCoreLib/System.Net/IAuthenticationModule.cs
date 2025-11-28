namespace System.Net;

public interface IAuthenticationModule
{
	string AuthenticationType { get; }

	bool CanPreAuthenticate { get; }

	Authorization? Authenticate(string challenge, WebRequest request, ICredentials credentials);

	Authorization? PreAuthenticate(WebRequest request, ICredentials credentials);
}
