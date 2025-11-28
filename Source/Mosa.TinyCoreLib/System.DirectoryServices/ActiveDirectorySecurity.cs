using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices;

public class ActiveDirectorySecurity : DirectoryObjectSecurity
{
	public override Type AccessRightType
	{
		get
		{
			throw null;
		}
	}

	public override Type AccessRuleType
	{
		get
		{
			throw null;
		}
	}

	public override Type AuditRuleType
	{
		get
		{
			throw null;
		}
	}

	public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		throw null;
	}

	public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type, Guid objectGuid, Guid inheritedObjectGuid)
	{
		throw null;
	}

	public void AddAccessRule(ActiveDirectoryAccessRule rule)
	{
	}

	public void AddAuditRule(ActiveDirectoryAuditRule rule)
	{
	}

	public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		throw null;
	}

	public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags, Guid objectGuid, Guid inheritedObjectGuid)
	{
		throw null;
	}

	public override bool ModifyAccessRule(AccessControlModification modification, AccessRule rule, out bool modified)
	{
		throw null;
	}

	public override bool ModifyAuditRule(AccessControlModification modification, AuditRule rule, out bool modified)
	{
		throw null;
	}

	public override void PurgeAccessRules(IdentityReference identity)
	{
	}

	public override void PurgeAuditRules(IdentityReference identity)
	{
	}

	public void RemoveAccess(IdentityReference identity, AccessControlType type)
	{
	}

	public bool RemoveAccessRule(ActiveDirectoryAccessRule rule)
	{
		throw null;
	}

	public void RemoveAccessRuleSpecific(ActiveDirectoryAccessRule rule)
	{
	}

	public void RemoveAudit(IdentityReference identity)
	{
	}

	public bool RemoveAuditRule(ActiveDirectoryAuditRule rule)
	{
		throw null;
	}

	public void RemoveAuditRuleSpecific(ActiveDirectoryAuditRule rule)
	{
	}

	public void ResetAccessRule(ActiveDirectoryAccessRule rule)
	{
	}

	public void SetAccessRule(ActiveDirectoryAccessRule rule)
	{
	}

	public void SetAuditRule(ActiveDirectoryAuditRule rule)
	{
	}
}
