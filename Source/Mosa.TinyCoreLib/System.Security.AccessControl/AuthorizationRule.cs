using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class AuthorizationRule
{
	protected internal int AccessMask
	{
		get
		{
			throw null;
		}
	}

	public IdentityReference IdentityReference
	{
		get
		{
			throw null;
		}
	}

	public InheritanceFlags InheritanceFlags
	{
		get
		{
			throw null;
		}
	}

	public bool IsInherited
	{
		get
		{
			throw null;
		}
	}

	public PropagationFlags PropagationFlags
	{
		get
		{
			throw null;
		}
	}

	protected internal AuthorizationRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
	}
}
