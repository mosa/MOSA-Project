using System.Collections;

namespace System.Net;

public class CredentialCache : IEnumerable, ICredentials, ICredentialsByHost
{
	public static ICredentials DefaultCredentials
	{
		get
		{
			throw null;
		}
	}

	public static NetworkCredential DefaultNetworkCredentials
	{
		get
		{
			throw null;
		}
	}

	public void Add(string host, int port, string authenticationType, NetworkCredential credential)
	{
	}

	public void Add(Uri uriPrefix, string authType, NetworkCredential cred)
	{
	}

	public NetworkCredential? GetCredential(string host, int port, string authenticationType)
	{
		throw null;
	}

	public NetworkCredential? GetCredential(Uri uriPrefix, string authType)
	{
		throw null;
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public void Remove(string? host, int port, string? authenticationType)
	{
	}

	public void Remove(Uri? uriPrefix, string? authType)
	{
	}
}
