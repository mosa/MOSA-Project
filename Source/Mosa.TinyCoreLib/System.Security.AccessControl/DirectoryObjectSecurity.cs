using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class DirectoryObjectSecurity : ObjectSecurity
{
	protected DirectoryObjectSecurity()
	{
	}

	protected DirectoryObjectSecurity(CommonSecurityDescriptor securityDescriptor)
	{
	}

	public virtual AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type, Guid objectType, Guid inheritedObjectType)
	{
		throw null;
	}

	protected void AddAccessRule(ObjectAccessRule rule)
	{
	}

	protected void AddAuditRule(ObjectAuditRule rule)
	{
	}

	public virtual AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags, Guid objectType, Guid inheritedObjectType)
	{
		throw null;
	}

	public AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType)
	{
		throw null;
	}

	public AuthorizationRuleCollection GetAuditRules(bool includeExplicit, bool includeInherited, Type targetType)
	{
		throw null;
	}

	protected override bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified)
	{
		throw null;
	}

	protected override bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified)
	{
		throw null;
	}

	protected bool RemoveAccessRule(ObjectAccessRule rule)
	{
		throw null;
	}

	protected void RemoveAccessRuleAll(ObjectAccessRule rule)
	{
	}

	protected void RemoveAccessRuleSpecific(ObjectAccessRule rule)
	{
	}

	protected bool RemoveAuditRule(ObjectAuditRule rule)
	{
		throw null;
	}

	protected void RemoveAuditRuleAll(ObjectAuditRule rule)
	{
	}

	protected void RemoveAuditRuleSpecific(ObjectAuditRule rule)
	{
	}

	protected void ResetAccessRule(ObjectAccessRule rule)
	{
	}

	protected void SetAccessRule(ObjectAccessRule rule)
	{
	}

	protected void SetAuditRule(ObjectAuditRule rule)
	{
	}
}
