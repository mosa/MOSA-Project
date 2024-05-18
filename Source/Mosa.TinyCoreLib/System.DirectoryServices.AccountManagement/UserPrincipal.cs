namespace System.DirectoryServices.AccountManagement;

[DirectoryRdnPrefix("CN")]
public class UserPrincipal : AuthenticablePrincipal
{
	public override AdvancedFilters AdvancedSearchFilter
	{
		get
		{
			throw null;
		}
	}

	public static UserPrincipal Current
	{
		get
		{
			throw null;
		}
	}

	public string EmailAddress
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string EmployeeId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string GivenName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string MiddleName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Surname
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string VoiceTelephoneNumber
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public UserPrincipal(PrincipalContext context)
		: base(null)
	{
	}

	public UserPrincipal(PrincipalContext context, string samAccountName, string password, bool enabled)
		: base(null)
	{
	}

	public new static PrincipalSearchResult<UserPrincipal> FindByBadPasswordAttempt(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public new static PrincipalSearchResult<UserPrincipal> FindByExpirationTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public new static UserPrincipal FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
	{
		throw null;
	}

	public new static UserPrincipal FindByIdentity(PrincipalContext context, string identityValue)
	{
		throw null;
	}

	public new static PrincipalSearchResult<UserPrincipal> FindByLockoutTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public new static PrincipalSearchResult<UserPrincipal> FindByLogonTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public new static PrincipalSearchResult<UserPrincipal> FindByPasswordSetTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public PrincipalSearchResult<Principal> GetAuthorizationGroups()
	{
		throw null;
	}
}
