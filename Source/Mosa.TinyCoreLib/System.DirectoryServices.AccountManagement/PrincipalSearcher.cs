namespace System.DirectoryServices.AccountManagement;

public class PrincipalSearcher : IDisposable
{
	public PrincipalContext Context
	{
		get
		{
			throw null;
		}
	}

	public Principal QueryFilter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PrincipalSearcher()
	{
	}

	public PrincipalSearcher(Principal queryFilter)
	{
	}

	public virtual void Dispose()
	{
	}

	public PrincipalSearchResult<Principal> FindAll()
	{
		throw null;
	}

	public Principal FindOne()
	{
		throw null;
	}

	public object GetUnderlyingSearcher()
	{
		throw null;
	}

	public Type GetUnderlyingSearcherType()
	{
		throw null;
	}
}
