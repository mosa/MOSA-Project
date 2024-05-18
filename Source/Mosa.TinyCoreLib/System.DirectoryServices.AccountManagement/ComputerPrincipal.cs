namespace System.DirectoryServices.AccountManagement;

[DirectoryRdnPrefix("CN")]
public class ComputerPrincipal : AuthenticablePrincipal
{
	public PrincipalValueCollection<string> ServicePrincipalNames
	{
		get
		{
			throw null;
		}
	}

	public ComputerPrincipal(PrincipalContext context)
		: base(null)
	{
	}

	public ComputerPrincipal(PrincipalContext context, string samAccountName, string password, bool enabled)
		: base(null)
	{
	}

	public new static PrincipalSearchResult<ComputerPrincipal> FindByBadPasswordAttempt(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public new static PrincipalSearchResult<ComputerPrincipal> FindByExpirationTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public new static ComputerPrincipal FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
	{
		throw null;
	}

	public new static ComputerPrincipal FindByIdentity(PrincipalContext context, string identityValue)
	{
		throw null;
	}

	public new static PrincipalSearchResult<ComputerPrincipal> FindByLockoutTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public new static PrincipalSearchResult<ComputerPrincipal> FindByLogonTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public new static PrincipalSearchResult<ComputerPrincipal> FindByPasswordSetTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}
}
