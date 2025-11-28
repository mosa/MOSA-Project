using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class AccessRule : AuthorizationRule
{
	public AccessControlType AccessControlType
	{
		get
		{
			throw null;
		}
	}

	protected AccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None)
	{
	}
}
public class AccessRule<T> : AccessRule where T : struct
{
	public T Rights
	{
		get
		{
			throw null;
		}
	}

	public AccessRule(IdentityReference identity, T rights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public AccessRule(IdentityReference identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public AccessRule(string identity, T rights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public AccessRule(string identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}
}
