using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class FileSystemSecurity : NativeObjectSecurity
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

	internal FileSystemSecurity()
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		throw null;
	}

	public void AddAccessRule(FileSystemAccessRule rule)
	{
	}

	public void AddAuditRule(FileSystemAuditRule rule)
	{
	}

	public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		throw null;
	}

	public bool RemoveAccessRule(FileSystemAccessRule rule)
	{
		throw null;
	}

	public void RemoveAccessRuleAll(FileSystemAccessRule rule)
	{
	}

	public void RemoveAccessRuleSpecific(FileSystemAccessRule rule)
	{
	}

	public bool RemoveAuditRule(FileSystemAuditRule rule)
	{
		throw null;
	}

	public void RemoveAuditRuleAll(FileSystemAuditRule rule)
	{
	}

	public void RemoveAuditRuleSpecific(FileSystemAuditRule rule)
	{
	}

	public void ResetAccessRule(FileSystemAccessRule rule)
	{
	}

	public void SetAccessRule(FileSystemAccessRule rule)
	{
	}

	public void SetAuditRule(FileSystemAuditRule rule)
	{
	}
}
