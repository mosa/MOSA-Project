namespace System.DirectoryServices.AccountManagement;

[DirectoryRdnPrefix("CN")]
public class GroupPrincipal : Principal
{
	public GroupScope? GroupScope
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool? IsSecurityGroup
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PrincipalCollection Members
	{
		get
		{
			throw null;
		}
	}

	public GroupPrincipal(PrincipalContext context)
	{
	}

	public GroupPrincipal(PrincipalContext context, string samAccountName)
	{
	}

	public override void Dispose()
	{
	}

	public new static GroupPrincipal FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
	{
		throw null;
	}

	public new static GroupPrincipal FindByIdentity(PrincipalContext context, string identityValue)
	{
		throw null;
	}

	public PrincipalSearchResult<Principal> GetMembers()
	{
		throw null;
	}

	public PrincipalSearchResult<Principal> GetMembers(bool recursive)
	{
		throw null;
	}
}
