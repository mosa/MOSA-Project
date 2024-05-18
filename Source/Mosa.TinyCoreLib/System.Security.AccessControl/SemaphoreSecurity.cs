using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class SemaphoreSecurity : NativeObjectSecurity
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

	public SemaphoreSecurity()
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public SemaphoreSecurity(string name, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		throw null;
	}

	public void AddAccessRule(SemaphoreAccessRule rule)
	{
	}

	public void AddAuditRule(SemaphoreAuditRule rule)
	{
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		throw null;
	}

	public bool RemoveAccessRule(SemaphoreAccessRule rule)
	{
		throw null;
	}

	public void RemoveAccessRuleAll(SemaphoreAccessRule rule)
	{
	}

	public void RemoveAccessRuleSpecific(SemaphoreAccessRule rule)
	{
	}

	public bool RemoveAuditRule(SemaphoreAuditRule rule)
	{
		throw null;
	}

	public void RemoveAuditRuleAll(SemaphoreAuditRule rule)
	{
	}

	public void RemoveAuditRuleSpecific(SemaphoreAuditRule rule)
	{
	}

	public void ResetAccessRule(SemaphoreAccessRule rule)
	{
	}

	public void SetAccessRule(SemaphoreAccessRule rule)
	{
	}

	public void SetAuditRule(SemaphoreAuditRule rule)
	{
	}
}
