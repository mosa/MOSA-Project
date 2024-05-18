using System.Security.Cryptography.X509Certificates;

namespace System.DirectoryServices.AccountManagement;

[DirectoryRdnPrefix("CN")]
public class AuthenticablePrincipal : Principal
{
	public DateTime? AccountExpirationDate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime? AccountLockoutTime
	{
		get
		{
			throw null;
		}
	}

	public virtual AdvancedFilters AdvancedSearchFilter
	{
		get
		{
			throw null;
		}
	}

	public bool AllowReversiblePasswordEncryption
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int BadLogonCount
	{
		get
		{
			throw null;
		}
	}

	public X509Certificate2Collection Certificates
	{
		get
		{
			throw null;
		}
	}

	public bool DelegationPermitted
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool? Enabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string HomeDirectory
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string HomeDrive
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime? LastBadPasswordAttempt
	{
		get
		{
			throw null;
		}
	}

	public DateTime? LastLogon
	{
		get
		{
			throw null;
		}
	}

	public DateTime? LastPasswordSet
	{
		get
		{
			throw null;
		}
	}

	public bool PasswordNeverExpires
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool PasswordNotRequired
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] PermittedLogonTimes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PrincipalValueCollection<string> PermittedWorkstations
	{
		get
		{
			throw null;
		}
	}

	public string ScriptPath
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SmartcardLogonRequired
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool UserCannotChangePassword
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected internal AuthenticablePrincipal(PrincipalContext context)
	{
	}

	protected internal AuthenticablePrincipal(PrincipalContext context, string samAccountName, string password, bool enabled)
	{
	}

	public void ChangePassword(string oldPassword, string newPassword)
	{
	}

	public void ExpirePasswordNow()
	{
	}

	public static PrincipalSearchResult<AuthenticablePrincipal> FindByBadPasswordAttempt(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	protected static PrincipalSearchResult<T> FindByBadPasswordAttempt<T>(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public static PrincipalSearchResult<AuthenticablePrincipal> FindByExpirationTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	protected static PrincipalSearchResult<T> FindByExpirationTime<T>(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public static PrincipalSearchResult<AuthenticablePrincipal> FindByLockoutTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	protected static PrincipalSearchResult<T> FindByLockoutTime<T>(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public static PrincipalSearchResult<AuthenticablePrincipal> FindByLogonTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	protected static PrincipalSearchResult<T> FindByLogonTime<T>(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public static PrincipalSearchResult<AuthenticablePrincipal> FindByPasswordSetTime(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	protected static PrincipalSearchResult<T> FindByPasswordSetTime<T>(PrincipalContext context, DateTime time, MatchType type)
	{
		throw null;
	}

	public bool IsAccountLockedOut()
	{
		throw null;
	}

	public void RefreshExpiredPassword()
	{
	}

	public void SetPassword(string newPassword)
	{
	}

	public void UnlockAccount()
	{
	}
}
