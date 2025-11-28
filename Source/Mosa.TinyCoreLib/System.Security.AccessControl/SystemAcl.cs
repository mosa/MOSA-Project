using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class SystemAcl : CommonAcl
{
	public SystemAcl(bool isContainer, bool isDS, byte revision, int capacity)
	{
	}

	public SystemAcl(bool isContainer, bool isDS, int capacity)
	{
	}

	public SystemAcl(bool isContainer, bool isDS, RawAcl rawAcl)
	{
	}

	public void AddAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
	}

	public void AddAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
	}

	public void AddAudit(SecurityIdentifier sid, ObjectAuditRule rule)
	{
	}

	public bool RemoveAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		throw null;
	}

	public bool RemoveAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		throw null;
	}

	public bool RemoveAudit(SecurityIdentifier sid, ObjectAuditRule rule)
	{
		throw null;
	}

	public void RemoveAuditSpecific(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
	}

	public void RemoveAuditSpecific(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
	}

	public void RemoveAuditSpecific(SecurityIdentifier sid, ObjectAuditRule rule)
	{
	}

	public void SetAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
	}

	public void SetAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
	}

	public void SetAudit(SecurityIdentifier sid, ObjectAuditRule rule)
	{
	}
}
