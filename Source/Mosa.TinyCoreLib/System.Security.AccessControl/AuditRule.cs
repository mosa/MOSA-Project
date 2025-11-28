using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class AuditRule : AuthorizationRule
{
	public AuditFlags AuditFlags
	{
		get
		{
			throw null;
		}
	}

	protected AuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None)
	{
	}
}
public class AuditRule<T> : AuditRule where T : struct
{
	public T Rights
	{
		get
		{
			throw null;
		}
	}

	public AuditRule(IdentityReference identity, T rights, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}

	public AuditRule(IdentityReference identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}

	public AuditRule(string identity, T rights, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}

	public AuditRule(string identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}
}
