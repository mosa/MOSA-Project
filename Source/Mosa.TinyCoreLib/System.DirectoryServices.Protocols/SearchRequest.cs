using System.Collections.Specialized;

namespace System.DirectoryServices.Protocols;

public class SearchRequest : DirectoryRequest
{
	public DereferenceAlias Aliases
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringCollection Attributes
	{
		get
		{
			throw null;
		}
	}

	public string DistinguishedName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object Filter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SearchScope Scope
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int SizeLimit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan TimeLimit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool TypesOnly
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SearchRequest()
	{
	}

	public SearchRequest(string distinguishedName, string ldapFilter, SearchScope searchScope, params string[] attributeList)
	{
	}
}
