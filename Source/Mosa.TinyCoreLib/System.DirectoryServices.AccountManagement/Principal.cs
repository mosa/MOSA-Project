using System.ComponentModel;
using System.Security.Principal;

namespace System.DirectoryServices.AccountManagement;

public abstract class Principal : IDisposable
{
	public PrincipalContext Context
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected internal PrincipalContext ContextRaw
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ContextType ContextType
	{
		get
		{
			throw null;
		}
	}

	public string Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string DisplayName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string DistinguishedName
	{
		get
		{
			throw null;
		}
	}

	public Guid? Guid
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
		set
		{
		}
	}

	public string SamAccountName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SecurityIdentifier Sid
	{
		get
		{
			throw null;
		}
	}

	public string StructuralObjectClass
	{
		get
		{
			throw null;
		}
	}

	public string UserPrincipalName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	protected Principal()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	protected void CheckDisposedOrDeleted()
	{
	}

	public void Delete()
	{
	}

	public virtual void Dispose()
	{
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	protected object[] ExtensionGet(string attribute)
	{
		throw null;
	}

	protected void ExtensionSet(string attribute, object value)
	{
	}

	public static Principal FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
	{
		throw null;
	}

	public static Principal FindByIdentity(PrincipalContext context, string identityValue)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	protected static Principal FindByIdentityWithType(PrincipalContext context, Type principalType, IdentityType identityType, string identityValue)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	protected static Principal FindByIdentityWithType(PrincipalContext context, Type principalType, string identityValue)
	{
		throw null;
	}

	public PrincipalSearchResult<Principal> GetGroups()
	{
		throw null;
	}

	public PrincipalSearchResult<Principal> GetGroups(PrincipalContext contextToQuery)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public object GetUnderlyingObject()
	{
		throw null;
	}

	public Type GetUnderlyingObjectType()
	{
		throw null;
	}

	public bool IsMemberOf(GroupPrincipal group)
	{
		throw null;
	}

	public bool IsMemberOf(PrincipalContext context, IdentityType identityType, string identityValue)
	{
		throw null;
	}

	public void Save()
	{
	}

	public void Save(PrincipalContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
