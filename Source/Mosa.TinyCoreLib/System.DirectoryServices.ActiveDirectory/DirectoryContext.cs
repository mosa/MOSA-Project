namespace System.DirectoryServices.ActiveDirectory;

public class DirectoryContext
{
	public DirectoryContextType ContextType
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public string? UserName
	{
		get
		{
			throw null;
		}
	}

	public DirectoryContext(DirectoryContextType contextType)
	{
	}

	public DirectoryContext(DirectoryContextType contextType, string name)
	{
	}

	public DirectoryContext(DirectoryContextType contextType, string? username, string? password)
	{
	}

	public DirectoryContext(DirectoryContextType contextType, string name, string? username, string? password)
	{
	}
}
