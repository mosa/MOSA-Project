using System.Collections.Generic;
using System.Security.Claims;

namespace System.Security.Principal;

public class WindowsPrincipal : ClaimsPrincipal
{
	public virtual IEnumerable<Claim> DeviceClaims
	{
		get
		{
			throw null;
		}
	}

	public override IIdentity Identity
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<Claim> UserClaims
	{
		get
		{
			throw null;
		}
	}

	public WindowsPrincipal(WindowsIdentity ntIdentity)
	{
	}

	public virtual bool IsInRole(int rid)
	{
		throw null;
	}

	public virtual bool IsInRole(SecurityIdentifier sid)
	{
		throw null;
	}

	public virtual bool IsInRole(WindowsBuiltInRole role)
	{
		throw null;
	}

	public override bool IsInRole(string role)
	{
		throw null;
	}
}
