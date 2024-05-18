using System.Collections;
using System.Collections.Specialized;

namespace System.Net;

public class AuthenticationManager
{
	public static ICredentialPolicy? CredentialPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static StringDictionary CustomTargetNameDictionary
	{
		get
		{
			throw null;
		}
	}

	public static IEnumerator RegisteredModules
	{
		get
		{
			throw null;
		}
	}

	internal AuthenticationManager()
	{
	}

	[Obsolete("The AuthenticationManager Authenticate and PreAuthenticate methods are not supported and throw PlatformNotSupportedException.", DiagnosticId = "SYSLIB0009", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static Authorization? Authenticate(string challenge, WebRequest request, ICredentials credentials)
	{
		throw null;
	}

	[Obsolete("The AuthenticationManager Authenticate and PreAuthenticate methods are not supported and throw PlatformNotSupportedException.", DiagnosticId = "SYSLIB0009", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static Authorization? PreAuthenticate(WebRequest request, ICredentials credentials)
	{
		throw null;
	}

	public static void Register(IAuthenticationModule authenticationModule)
	{
	}

	public static void Unregister(IAuthenticationModule authenticationModule)
	{
	}

	public static void Unregister(string authenticationScheme)
	{
	}
}
