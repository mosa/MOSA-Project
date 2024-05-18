using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class ObjectAuditRule : AuditRule
{
	public Guid InheritedObjectType
	{
		get
		{
			throw null;
		}
	}

	public ObjectAceFlags ObjectFlags
	{
		get
		{
			throw null;
		}
	}

	public Guid ObjectType
	{
		get
		{
			throw null;
		}
	}

	protected ObjectAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AuditFlags auditFlags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}
}
