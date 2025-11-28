using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.DirectoryServices;

public class DirectorySearcher : Component
{
	[DefaultValue(false)]
	public bool Asynchronous
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue("")]
	public string AttributeScopeQuery
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	[DefaultValue(true)]
	public bool CacheResults
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan ClientTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(DereferenceAlias.Never)]
	public DereferenceAlias DerefAlias
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	public DirectorySynchronization? DirectorySynchronization
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(ExtendedDN.None)]
	public ExtendedDN ExtendedDN
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue("(objectClass=*)")]
	public string? Filter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(0)]
	public int PageSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public StringCollection PropertiesToLoad
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(false)]
	public bool PropertyNamesOnly
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(ReferralChasingOption.External)]
	public ReferralChasingOption ReferralChasing
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	public DirectoryEntry? SearchRoot
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(SearchScope.Subtree)]
	public SearchScope SearchScope
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(SecurityMasks.None)]
	public SecurityMasks SecurityMasks
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan ServerPageTimeLimit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan ServerTimeLimit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(0)]
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

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public SortOption Sort
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	public bool Tombstone
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	public DirectoryVirtualListView? VirtualListView
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectorySearcher()
	{
	}

	public DirectorySearcher(DirectoryEntry? searchRoot)
	{
	}

	public DirectorySearcher(DirectoryEntry? searchRoot, string? filter)
	{
	}

	public DirectorySearcher(DirectoryEntry? searchRoot, string? filter, string[]? propertiesToLoad)
	{
	}

	public DirectorySearcher(DirectoryEntry? searchRoot, string? filter, string[]? propertiesToLoad, SearchScope scope)
	{
	}

	public DirectorySearcher(string? filter)
	{
	}

	public DirectorySearcher(string? filter, string[]? propertiesToLoad)
	{
	}

	public DirectorySearcher(string? filter, string[]? propertiesToLoad, SearchScope scope)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public SearchResultCollection FindAll()
	{
		throw null;
	}

	public SearchResult? FindOne()
	{
		throw null;
	}
}
