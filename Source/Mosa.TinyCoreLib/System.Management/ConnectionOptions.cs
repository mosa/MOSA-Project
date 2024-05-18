using System.Security;

namespace System.Management;

public class ConnectionOptions : ManagementOptions
{
	public AuthenticationLevel Authentication
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Authority
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool EnablePrivileges
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ImpersonationLevel Impersonation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Locale
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Password
	{
		set
		{
		}
	}

	public SecureString SecurePassword
	{
		set
		{
		}
	}

	public string Username
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConnectionOptions()
	{
	}

	public ConnectionOptions(string locale, string username, SecureString password, string authority, ImpersonationLevel impersonation, AuthenticationLevel authentication, bool enablePrivileges, ManagementNamedValueCollection context, TimeSpan timeout)
	{
	}

	public ConnectionOptions(string locale, string username, string password, string authority, ImpersonationLevel impersonation, AuthenticationLevel authentication, bool enablePrivileges, ManagementNamedValueCollection context, TimeSpan timeout)
	{
	}

	public override object Clone()
	{
		throw null;
	}
}
