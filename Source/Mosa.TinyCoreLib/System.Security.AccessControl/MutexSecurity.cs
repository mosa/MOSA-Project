using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class MutexSecurity : NativeObjectSecurity
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

	public MutexSecurity()
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public MutexSecurity(string name, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		throw null;
	}

	public void AddAccessRule(MutexAccessRule rule)
	{
	}

	public void AddAuditRule(MutexAuditRule rule)
	{
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		throw null;
	}

	public bool RemoveAccessRule(MutexAccessRule rule)
	{
		throw null;
	}

	public void RemoveAccessRuleAll(MutexAccessRule rule)
	{
	}

	public void RemoveAccessRuleSpecific(MutexAccessRule rule)
	{
	}

	public bool RemoveAuditRule(MutexAuditRule rule)
	{
		throw null;
	}

	public void RemoveAuditRuleAll(MutexAuditRule rule)
	{
	}

	public void RemoveAuditRuleSpecific(MutexAuditRule rule)
	{
	}

	public void ResetAccessRule(MutexAccessRule rule)
	{
	}

	public void SetAccessRule(MutexAccessRule rule)
	{
	}

	public void SetAuditRule(MutexAuditRule rule)
	{
	}
}
