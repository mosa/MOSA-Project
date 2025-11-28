using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class RegistrySecurity : NativeObjectSecurity
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

	public RegistrySecurity()
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		throw null;
	}

	public void AddAccessRule(RegistryAccessRule rule)
	{
	}

	public void AddAuditRule(RegistryAuditRule rule)
	{
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		throw null;
	}

	public bool RemoveAccessRule(RegistryAccessRule rule)
	{
		throw null;
	}

	public void RemoveAccessRuleAll(RegistryAccessRule rule)
	{
	}

	public void RemoveAccessRuleSpecific(RegistryAccessRule rule)
	{
	}

	public bool RemoveAuditRule(RegistryAuditRule rule)
	{
		throw null;
	}

	public void RemoveAuditRuleAll(RegistryAuditRule rule)
	{
	}

	public void RemoveAuditRuleSpecific(RegistryAuditRule rule)
	{
	}

	public void ResetAccessRule(RegistryAccessRule rule)
	{
	}

	public void SetAccessRule(RegistryAccessRule rule)
	{
	}

	public void SetAuditRule(RegistryAuditRule rule)
	{
	}
}
