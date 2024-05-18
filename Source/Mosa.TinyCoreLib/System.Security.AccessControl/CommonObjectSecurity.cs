namespace System.Security.AccessControl;

public abstract class CommonObjectSecurity : ObjectSecurity
{
	protected CommonObjectSecurity(bool isContainer)
	{
	}

	protected void AddAccessRule(AccessRule rule)
	{
	}

	protected void AddAuditRule(AuditRule rule)
	{
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

	protected bool RemoveAccessRule(AccessRule rule)
	{
		throw null;
	}

	protected void RemoveAccessRuleAll(AccessRule rule)
	{
	}

	protected void RemoveAccessRuleSpecific(AccessRule rule)
	{
	}

	protected bool RemoveAuditRule(AuditRule rule)
	{
		throw null;
	}

	protected void RemoveAuditRuleAll(AuditRule rule)
	{
	}

	protected void RemoveAuditRuleSpecific(AuditRule rule)
	{
	}

	protected void ResetAccessRule(AccessRule rule)
	{
	}

	protected void SetAccessRule(AccessRule rule)
	{
	}

	protected void SetAuditRule(AuditRule rule)
	{
	}
}
