using System.Net;

namespace System.DirectoryServices.Protocols;

public class LdapConnection : DirectoryConnection, IDisposable
{
	public AuthType AuthType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool AutoBind
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override NetworkCredential Credential
	{
		set
		{
		}
	}

	public LdapSessionOptions SessionOptions
	{
		get
		{
			throw null;
		}
	}

	public override TimeSpan Timeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public LdapConnection(LdapDirectoryIdentifier identifier)
	{
	}

	public LdapConnection(LdapDirectoryIdentifier identifier, NetworkCredential credential)
	{
	}

	public LdapConnection(LdapDirectoryIdentifier identifier, NetworkCredential credential, AuthType authType)
	{
	}

	public LdapConnection(string server)
	{
	}

	public void Abort(IAsyncResult asyncResult)
	{
	}

	public IAsyncResult BeginSendRequest(DirectoryRequest request, PartialResultProcessing partialMode, AsyncCallback callback, object state)
	{
		throw null;
	}

	public IAsyncResult BeginSendRequest(DirectoryRequest request, TimeSpan requestTimeout, PartialResultProcessing partialMode, AsyncCallback callback, object state)
	{
		throw null;
	}

	public void Bind()
	{
	}

	public void Bind(NetworkCredential newCredential)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public DirectoryResponse EndSendRequest(IAsyncResult asyncResult)
	{
		throw null;
	}

	~LdapConnection()
	{
	}

	public PartialResultsCollection GetPartialResults(IAsyncResult asyncResult)
	{
		throw null;
	}

	public override DirectoryResponse SendRequest(DirectoryRequest request)
	{
		throw null;
	}

	public DirectoryResponse SendRequest(DirectoryRequest request, TimeSpan requestTimeout)
	{
		throw null;
	}
}
