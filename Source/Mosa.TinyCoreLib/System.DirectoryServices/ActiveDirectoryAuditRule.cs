using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices;

public class ActiveDirectoryAuditRule : ObjectAuditRule
{
	public ActiveDirectoryRights ActiveDirectoryRights
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectorySecurityInheritance InheritanceType
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AuditFlags.None)
	{
	}

	public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AuditFlags.None)
	{
	}

	public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AuditFlags.None)
	{
	}

	public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, Guid objectType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AuditFlags.None)
	{
	}

	public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, Guid objectType, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AuditFlags.None)
	{
	}

	public ActiveDirectoryAuditRule(IdentityReference identity, ActiveDirectoryRights adRights, AuditFlags auditFlags, Guid objectType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AuditFlags.None)
	{
	}
}
