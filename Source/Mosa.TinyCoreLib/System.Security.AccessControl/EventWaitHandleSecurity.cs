using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class EventWaitHandleSecurity : NativeObjectSecurity
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

	public EventWaitHandleSecurity()
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		throw null;
	}

	public void AddAccessRule(EventWaitHandleAccessRule rule)
	{
	}

	public void AddAuditRule(EventWaitHandleAuditRule rule)
	{
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		throw null;
	}

	public bool RemoveAccessRule(EventWaitHandleAccessRule rule)
	{
		throw null;
	}

	public void RemoveAccessRuleAll(EventWaitHandleAccessRule rule)
	{
	}

	public void RemoveAccessRuleSpecific(EventWaitHandleAccessRule rule)
	{
	}

	public bool RemoveAuditRule(EventWaitHandleAuditRule rule)
	{
		throw null;
	}

	public void RemoveAuditRuleAll(EventWaitHandleAuditRule rule)
	{
	}

	public void RemoveAuditRuleSpecific(EventWaitHandleAuditRule rule)
	{
	}

	public void ResetAccessRule(EventWaitHandleAccessRule rule)
	{
	}

	public void SetAccessRule(EventWaitHandleAccessRule rule)
	{
	}

	public void SetAuditRule(EventWaitHandleAuditRule rule)
	{
	}
}
