namespace System.DirectoryServices.AccountManagement;

public class PrincipalContext : IDisposable
{
	public string ConnectedServer
	{
		get
		{
			throw null;
		}
	}

	public string Container
	{
		get
		{
			throw null;
		}
	}

	public ContextType ContextType
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public ContextOptions Options
	{
		get
		{
			throw null;
		}
	}

	public string UserName
	{
		get
		{
			throw null;
		}
	}

	public PrincipalContext(ContextType contextType)
	{
	}

	public PrincipalContext(ContextType contextType, string name)
	{
	}

	public PrincipalContext(ContextType contextType, string name, string container)
	{
	}

	public PrincipalContext(ContextType contextType, string name, string container, ContextOptions options)
	{
	}

	public PrincipalContext(ContextType contextType, string name, string container, ContextOptions options, string userName, string password)
	{
	}

	public PrincipalContext(ContextType contextType, string name, string userName, string password)
	{
	}

	public PrincipalContext(ContextType contextType, string name, string container, string userName, string password)
	{
	}

	public void Dispose()
	{
	}

	public bool ValidateCredentials(string userName, string password)
	{
		throw null;
	}

	public bool ValidateCredentials(string userName, string password, ContextOptions options)
	{
		throw null;
	}
}
